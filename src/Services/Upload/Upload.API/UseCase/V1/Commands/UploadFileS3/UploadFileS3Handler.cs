using Upload.API.Services.S3Upload;

namespace Upload.API.UseCase.V1.Commands.UploadFileS3;

public class UploadFileS3Handler
(IS3UploadService s3UploadService)
: ICommandHandler<UploadFileS3Command, FileResponseDto>
{
    public async Task<FileResponseDto> Handle(UploadFileS3Command request, CancellationToken cancellationToken)
    {
        var result = await s3UploadService.S3UploadFileAsync(request.Stream, cancellationToken);
        return new FileResponseDto(result, "");
    }
}