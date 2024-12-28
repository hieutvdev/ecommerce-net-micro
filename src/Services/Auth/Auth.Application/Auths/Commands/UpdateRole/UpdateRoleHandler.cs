using Auth.Application.DTOs.Roles.Requests;

namespace Auth.Application.Auths.Commands.UpdateRole;

public class UpdateRoleHandler
(IRoleRepository roleRepository)
: ICommandHandler<UpdateRoleCommand, UpdateRoleResult>
{
    public async Task<UpdateRoleResult> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var request = UpdateRoleCommandToDto(command);
        var result = await roleRepository.UpdateRoleAsync(request, cancellationToken);
        return new UpdateRoleResult(result);
    }


    private static UpdateRoleRequestDto UpdateRoleCommandToDto(UpdateRoleCommand command)
        => new UpdateRoleRequestDto(command.RoleId, command.RoleName);
}