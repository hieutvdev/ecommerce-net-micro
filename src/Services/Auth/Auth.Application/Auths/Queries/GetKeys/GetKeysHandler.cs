namespace Auth.Application.Auths.Queries.GetKeys;

public class GetKeysHandler
(IKeyRepository<Guid> keyRepository)
: IQueryHandler<GetKeysQuery, GetKeysResult>
{
    public async Task<GetKeysResult> Handle(GetKeysQuery request, CancellationToken cancellationToken)
    {
        var result = await keyRepository.GetKeysAsync(request.PaginationRequest, cancellationToken);
        return new GetKeysResult(result);
    }
}