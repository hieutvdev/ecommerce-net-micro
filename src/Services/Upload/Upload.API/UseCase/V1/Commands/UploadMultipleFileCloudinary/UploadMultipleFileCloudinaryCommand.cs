


namespace Upload.API.UseCase.V1.Commands.UploadMultipleFileCloudinary;

public record UploadMultipleFileCloudinaryCommand(IReadOnlyList<IFormFile> Files) : ICommand<IEnumerable<FileResponseDto>>;