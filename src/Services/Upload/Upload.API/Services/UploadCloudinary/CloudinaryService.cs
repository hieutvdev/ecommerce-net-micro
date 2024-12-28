using System.Net;
using BuildingBlocks.Exceptions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Serilog;
using Upload.API.Configuration;
using Upload.API.Dtos;
using static System.Drawing.Image;

namespace Upload.API.Services.UploadCloudinary;

public class CloudinaryService : ICloudinaryService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;
    
    private readonly long _maxFileSize = 1 * 1024 * 1024;
    
    private readonly string[] _allowedExtensions = new string[]
    {
        ".jpg", ".jpeg", ".png", ".gif", ".pdf" 
    };

    private bool IsStreamAnImage(Stream stream)
    {
        try
        {
            using var image = FromStream(stream);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    
    private string GenerateFileName()
        => $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}";

    private string CreateImageUrl(string publicId, int width, int height, bool highRes)
    {
        var transformation = highRes
            ? new Transformation().Width(width).Height(height).Crop("fill")
            : new Transformation().Width(width).Height(height).Crop("fill").Quality(50);
        
        return _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);
    }
    
    public CloudinaryService(IOptions<CloudinaryConfigurationSetting> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret);
        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
    }

    private bool CheckConditionFileImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new BadRequestException("File is missing");

        if (file.Length > _maxFileSize)
            throw new BadRequestException("File cannot upload Maxsize");

        if (!_allowedExtensions.Contains(Path.GetExtension(file.FileName)))
            throw new BadRequestException("File cannot is image");

        return true;

    }


    public async Task<FileResponseDto> UploadFileCloudinaryAsync(Stream fileStream, CancellationToken cancellationToken = default)
    {
        if (fileStream == null || fileStream.Length == 0)
            throw new BadRequestException("File is missing");

        if (fileStream.Length > _maxFileSize)
            throw new BadRequestException("File cannot upload Maxsize");

        if (!IsStreamAnImage(fileStream))
            throw new BadRequestException("File cannot be an image");
        if (fileStream.CanSeek)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
        }
        try
        {
            string fileName = GenerateFileName();
        
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream),
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };
            

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error is not null)
                throw new BadRequestException(uploadResult.Error.Message);

            var smallUrl = CreateImageUrl(uploadResult.PublicId, uploadResult.Width / 8, uploadResult.Height / 8, true);

            return new FileResponseDto(uploadResult.Url.ToString(), smallUrl);
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while uploading to Cloudinary");
            throw new BadRequestException("Upload file cloudinary error!!!!");
        }
    }

    public async Task<IEnumerable<FileResponseDto>> UploadMultipleFileCloudinaryAsync(IReadOnlyList<IFormFile> fileStreams, CancellationToken cancellationToken = default)
    {
        var uploadFiles = new List<FileResponseDto>();

        foreach (var file in fileStreams)
        {
            var fileName = GenerateFileName();
            if (CheckConditionFileImage(file))
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(fileName, stream)
                    };
                    var upload = await Task.Run(() => _cloudinary.UploadAsync(uploadParams), cancellationToken);
                    uploadFiles.Add(new FileResponseDto(upload.Url.ToString(), CreateImageUrl(upload.PublicId, upload.Width /8, upload.Height/8, true)));
                }
            }
        }
        return uploadFiles;

    }

    public async Task<bool> DeleteFileAsync(string publicId, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = publicId.Split("/");
            var id = url[^1].Split(".")[0];
            if (string.IsNullOrEmpty(id))
                throw new BadRequestException("publicIc cannot be null or empty");
            
            var deleteParams = new DeletionParams(id);
            var result = await Task.Run(() => _cloudinary.DestroyAsync(deleteParams), cancellationToken);

            return result.StatusCode == HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}