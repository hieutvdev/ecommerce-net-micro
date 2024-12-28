using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Upload.API.Dtos;
using Upload.API.Services;

namespace Upload.API.UseCase.V1.Commands.UploadFileFromLocal;

public class UploadFileFromLocalHandler
(IUploadFileService uploadFileService)
: ICommandHandler<UploadFileFromLocalCommand, FileResponseDto>
{
    
    public async Task<FileResponseDto> Handle(UploadFileFromLocalCommand request, CancellationToken cancellationToken)
    {
        var fromCollection = request.Context.Request.ReadFormAsync(cancellationToken);
        var file = fromCollection.Result.Files.GetFile("file");
        if (file is null)
            throw new BadRequestException("File is missing");
        var result = await uploadFileService.UploadFileAsync(file);
        return result;
    }
}