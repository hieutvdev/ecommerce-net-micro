using BuildingBlocks.CQRS;
using Upload.API.Dtos;

namespace Upload.API.UseCase.V1.Commands.UploadFileFromLocal;

public record UploadFileFromLocalCommand(HttpContext Context) : ICommand<FileResponseDto>;