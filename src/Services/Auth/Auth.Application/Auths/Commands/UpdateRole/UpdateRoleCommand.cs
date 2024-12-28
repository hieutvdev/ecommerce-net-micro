namespace Auth.Application.Auths.Commands.UpdateRole;

public record UpdateRoleCommand(string RoleId, string RoleName) : ICommand<UpdateRoleResult>;
public record UpdateRoleResult(bool IsSuccess);