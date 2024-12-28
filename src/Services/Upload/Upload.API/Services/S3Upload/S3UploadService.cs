using Amazon.CloudFront;
using Amazon.CloudFront.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Exceptions;
using Serilog;

namespace Upload.API.Services.S3Upload;

public class S3UploadService : IS3UploadService
{
    private readonly IAmazonS3 _amazonS3;
    private readonly string? _bucketName;
    private readonly string? _cloudFrontUrl;
    private readonly string? _cloudFrontKeyPairId;
    private readonly string? _privateKey;

    public S3UploadService(IConfiguration configuration, IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
        _bucketName = configuration["S3:BucketName"];
        _cloudFrontUrl = configuration["CloudFront:Url"];
        _cloudFrontKeyPairId = configuration["CloudFront:KeyPairId"];
        _privateKey = configuration["CloudFront:PrivateKey"];
    }
    
    private string GenerateFileName()
        => $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}";



    private string GenerateCloudFrontUrl(string fileName) => $"https://{_cloudFrontUrl}/{fileName}";
   

    private string CleanBase64String(string pemString)
    {
        var base64String = pemString
            .Replace("\r", ""); // Remove carriage returns

        return base64String;
    }

    public async Task<string> S3UploadFileAsync(Stream fileStream, CancellationToken cancellationToken = default)
    {
        if (fileStream.CanSeek)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
        }

        var fileTransferUtility = new TransferUtility(_amazonS3);
        var fileName = GenerateFileName(); 
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = _bucketName
        };

        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);

        var url = await RefreshExpireImageUrl(fileName);

        return url;

    }


    public async Task<string> S3UploadGenerateCloudFrontAsync(Stream fileStream,
        CancellationToken cancellationToken = default)
    {
        if (fileStream.CanSeek)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
        }

        var fileTransferUtility = new TransferUtility(_amazonS3);
        var fileName = GenerateFileName();
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = _bucketName
        };

        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);

        try
        {
           
            var url = AmazonCloudFrontUrlSigner.GetCannedSignedURL(
                _cloudFrontUrl + "/" + fileName,
                new StreamReader(@"./Keys/private_key.pem"),
                _cloudFrontKeyPairId,
                DateTime.UtcNow.AddDays(4)
            );
            return url;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Log.Error(e.Message);
            throw new BadRequestException(e.Message);
        }
        
    }

    private async Task<string> RefreshExpireImageUrl(string fileName)
    {
        var urlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.AddMinutes(24)
        };
        var signedUrl = await _amazonS3.GetPreSignedURLAsync(urlRequest);
        
        return signedUrl;
    }
    
    
}

