using BuildingBlocks.Exceptions;
using Upload.API.Services.UploadCloudinary;

namespace Upload.API.UseCase.V1.Commands.DeleteFileCloudinary;

public class DeleteFileCloudinaryHandler
    (ICloudinaryService cloudinaryService)
: ICommandHandler<DeleteFileCloudinaryCommand, bool>
{
    public async Task<bool> Handle(DeleteFileCloudinaryCommand request, CancellationToken cancellationToken)
    {
        var result = await cloudinaryService.DeleteFileAsync(request.PublicId, cancellationToken);


        return result;
    }
}