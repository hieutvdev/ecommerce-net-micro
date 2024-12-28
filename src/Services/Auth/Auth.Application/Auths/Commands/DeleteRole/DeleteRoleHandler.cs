using Auth.Application.DTOs.Roles.Requests;

namespace Auth.Application.Auths.Commands.DeleteRole;

public class DeleteRoleHandler
(IRoleRepository roleRepository)
: ICommandHandler<DeleteRoleCommand, DeleteRoleResult>
{
    public async Task<DeleteRoleResult> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var request = DeleteRoleCommandToDto(command);
        var result = await roleRepository.DeleteRoleAsync(request, cancellationToken);
        return new DeleteRoleResult(result);
    }

    private static DeleteRoleRequestDto DeleteRoleCommandToDto(DeleteRoleCommand command)
        => new DeleteRoleRequestDto(command.RoleName);
}