using Auth.Application.DTOs.Key;

namespace Auth.Application.Auths.Queries.GetKeys;

public record GetKeysQuery(PaginationRequest PaginationRequest) : IQuery<GetKeysResult>;

public record GetKeysResult(PaginatedResult<KeyDto> PaginatedResult);
