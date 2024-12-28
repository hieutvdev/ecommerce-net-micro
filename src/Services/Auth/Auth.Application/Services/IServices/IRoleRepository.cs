using Auth.Application.DTOs.Roles.Requests;
using Auth.Application.DTOs.Roles.Responses;

namespace Auth.Application.Services.IServices;

public interface IRoleRepository
{
    Task<IEnumerable<object>> GetRolesAsync(CancellationToken cancellationToken = default!);
    Task<bool> AssignRolesAsync(AssignRoleRequestDto dto, CancellationToken cancellationToken = default!);
    Task<bool> DeleteRoleAsync(DeleteRoleRequestDto dto, CancellationToken cancellationToken = default!);
    Task<bool> UpdateRoleAsync(UpdateRoleRequestDto dto, CancellationToken cancellationToken = default!);
    Task<bool> CreateRoleAsync(CreateRoleRequestDto dto, CancellationToken cancellationToken = default!);
}