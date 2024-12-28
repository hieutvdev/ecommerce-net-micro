namespace Auth.Application.Auths.Queries.GetUsers;

public class GetUsersHandler
(IUserRepository userRepository)
: IQueryHandler<GetUsersQuery, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetUsersAsync(request.PaginationRequest, cancellationToken);
        return new GetUsersResult(result);
    }
}