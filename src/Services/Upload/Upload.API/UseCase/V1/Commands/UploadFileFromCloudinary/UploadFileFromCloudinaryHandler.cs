using BuildingBlocks.CQRS;
using Upload.API.Dtos;
using Upload.API.Services.UploadCloudinary;

namespace Upload.API.UseCase.V1.Commands.UploadFileFromCloudinary;

public class UploadFileFromCloudinaryHandler
(ICloudinaryService cloudinaryService)
: ICommandHandler<UploadFileFromCloudinaryCommand, FileResponseDto>
{
    public async Task<FileResponseDto> Handle(UploadFileFromCloudinaryCommand request, CancellationToken cancellationToken)
    {
        var result = await cloudinaryService.UploadFileCloudinaryAsync(request.Stream, cancellationToken);

        return result;
    }
}