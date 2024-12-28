namespace Upload.API.UseCase.V1.Commands.DeleteFileCloudinary;

public record DeleteFileCloudinaryCommand(string PublicId) : ICommand<bool>;
