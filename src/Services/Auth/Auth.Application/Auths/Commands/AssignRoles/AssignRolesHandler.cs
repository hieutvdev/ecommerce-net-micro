using Auth.Application.DTOs.Roles.Requests;

namespace Auth.Application.Auths.Commands.AssignRoles;

public class AssignRolesHandler
(IRoleRepository roleRepository)
: ICommandHandler<AssignRolesCommand, AssignRolesResult>
{
    public async Task<AssignRolesResult> Handle(AssignRolesCommand command, CancellationToken cancellationToken)
    {
        var request = AssignRoleCommandToDto(command);
        var result = await roleRepository.AssignRolesAsync(request, cancellationToken);
        return new AssignRolesResult(result);
    }

    private static AssignRoleRequestDto AssignRoleCommandToDto(AssignRolesCommand command)
        => new AssignRoleRequestDto(command.RoleNames, command.Email);

}