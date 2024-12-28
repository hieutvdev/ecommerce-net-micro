namespace Auth.Application.Auths.Commands.DeleteRole;

public record DeleteRoleCommand(string RoleName) : ICommand<DeleteRoleResult>;

public record DeleteRoleResult(bool IsSuccess);