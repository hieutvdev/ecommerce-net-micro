using Auth.Application.DTOs.Users.Responses;
using BuildingBlocks.Pagination;

namespace Auth.Application.Auths.Queries.GetUsers;

public record GetUsersQuery(PaginationRequest PaginationRequest) : IQuery<GetUsersResult>;
public record GetUsersResult(PaginatedResult<GetUsersResponseDto> PaginatedResult);