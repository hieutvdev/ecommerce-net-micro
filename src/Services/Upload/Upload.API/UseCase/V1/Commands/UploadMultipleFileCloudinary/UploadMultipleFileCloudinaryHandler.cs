using Upload.API.Services.UploadCloudinary;
using Upload.API.UseCase.V1.Commands.UploadFileFromCloudinary;

namespace Upload.API.UseCase.V1.Commands.UploadMultipleFileCloudinary;

public class UploadMultipleFileCloudinaryHandler
    (ICloudinaryService cloudinaryService)
    : ICommandHandler<UploadMultipleFileCloudinaryCommand, IEnumerable<FileResponseDto>>
{
    public async Task<IEnumerable<FileResponseDto>> Handle(UploadMultipleFileCloudinaryCommand request, CancellationToken cancellationToken)
    {
        var result = await cloudinaryService.UploadMultipleFileCloudinaryAsync(request.Files, cancellationToken);

        return result;
    }
}