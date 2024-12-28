using BuildingBlocks.CQRS;
using Upload.API.Dtos;

namespace Upload.API.UseCase.V1.Commands.UploadFileFromCloudinary;

public record UploadFileFromCloudinaryCommand(Stream Stream) : ICommand<FileResponseDto>;
