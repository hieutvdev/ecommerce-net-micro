namespace Auth.Application.Auths.Commands.AssignRoles;

public record AssignRolesCommand(string[] RoleNames, string Email) : ICommand<AssignRolesResult>;
public record AssignRolesResult(bool IsSuccess);