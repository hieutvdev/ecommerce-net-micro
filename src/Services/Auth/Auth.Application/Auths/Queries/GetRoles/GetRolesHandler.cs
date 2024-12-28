namespace Auth.Application.Auths.Queries.GetRoles;

public class GetRolesHandler
(IRoleRepository roleRepository)
: IQueryHandler<GetRolesQuery, GetRolesResult>
{
    public async Task<GetRolesResult> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var result = await roleRepository.GetRolesAsync(cancellationToken);
        return new GetRolesResult(result);
    }
}