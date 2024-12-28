namespace Upload.API.UseCase.V1.Commands.UploadFileS3;

public record UploadFileS3Command(Stream Stream) : ICommand<FileResponseDto>;
