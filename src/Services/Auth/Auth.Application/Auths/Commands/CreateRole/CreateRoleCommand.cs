namespace Auth.Application.Auths.Commands.CreateRole;

public record CreateRoleCommand(string RoleName) : ICommand<CreateRoleResult>;
public record CreateRoleResult(bool IsSuccess);