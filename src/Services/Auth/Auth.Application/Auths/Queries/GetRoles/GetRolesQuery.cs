using Auth.Application.DTOs.Roles.Responses;

namespace Auth.Application.Auths.Queries.GetRoles;

public record GetRolesQuery() : IQuery<GetRolesResult>;

public record GetRolesResult(IEnumerable<object> Response);
