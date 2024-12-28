using Auth.Application.DTOs.Users.Requests;
using Auth.Application.DTOs.Users.Responses;
using BuildingBlocks.Pagination;

namespace Auth.Application.Services.IServices;

public interface IUserRepository
{
    Task<GetUserResponseDto> GetUserAsync(GetUserRequestDto dto, CancellationToken cancellationToken = default!);
    Task<GetUserResponseDto> EditInForUserAsync(EditInForUserRequestDto dto, CancellationToken cancellationToken = default!);
    Task<PaginatedResult<GetUsersResponseDto>> GetUsersAsync(PaginationRequest paginationRequest,CancellationToken cancellationToken = default!);

}