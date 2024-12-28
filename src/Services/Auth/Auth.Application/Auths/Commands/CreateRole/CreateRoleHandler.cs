using Auth.Application.DTOs.Roles.Requests;

namespace Auth.Application.Auths.Commands.CreateRole;

public class CreateRoleHandler
(IRoleRepository roleRepository)
: ICommandHandler<CreateRoleCommand, CreateRoleResult>
{
    public async Task<CreateRoleResult> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var request = CreateRoleCommandToDto(command);
        var result = await roleRepository.CreateRoleAsync(request, cancellationToken);
        return new CreateRoleResult(result);
    }


    private static CreateRoleRequestDto CreateRoleCommandToDto(CreateRoleCommand command)
        => new CreateRoleRequestDto(command.RoleName);
}