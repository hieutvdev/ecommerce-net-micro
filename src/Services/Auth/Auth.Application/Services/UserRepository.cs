using Auth.Application.DTOs.Users.Requests;
using Auth.Application.DTOs.Users.Responses;


namespace Auth.Application.Services;

public class UserRepository(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, ILogger<UserRepository> logger)
    : IUserRepository
{
    public async Task<GetUserResponseDto> GetUserAsync(GetUserRequestDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var userFound = await userManager.FindByIdAsync(dto.UserId) ?? throw new NotFoundException("User NotFound");
            logger.LogInformation("Get user repo: {id}", userFound.Id);
            return AuthExtensions.ApplicationUserToUserInFo(userFound);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<GetUserResponseDto> EditInForUserAsync(EditInForUserRequestDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var userFound = await userManager.FindByIdAsync(dto.UserId) ??
                            throw new NotFoundException("User NotFound");
            userFound.FullName = dto.FullName ?? userFound.FullName;
            userFound.PhoneNumber = dto.PhoneNumber ?? userFound.PhoneNumber;
            userFound.Avatar = dto.Avatar;
            var userUpdate = await userManager.UpdateAsync(userFound);
            if (!userUpdate.Succeeded)
            {
                throw new BadRequestException("Update User Failure");
            }
            return new GetUserResponseDto(userFound.Id, userFound.Email ?? "", userFound.UserName ?? "", dto.FullName ?? "",
                dto.PhoneNumber ?? "", dto.Avatar);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<GetUsersResponseDto>> GetUsersAsync(PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            int pageIndex = paginationRequest.PageIndex;
            int pageSize = paginationRequest.PageSize;
            long totalCount = await userManager.Users.LongCountAsync(cancellationToken);
            var users = await userManager.Users.Skip(pageSize * pageIndex).Take(pageSize)
                .ToListAsync(cancellationToken);
            var userRoles = users.Select(x => new GetUsersResponseDto(x.Email ?? "", x.FullName, x.PhoneNumber ?? "",
                x.Avatar, x.Status, Roles: userManager.GetRolesAsync(x).GetAwaiter().GetResult()));
            return new PaginatedResult<GetUsersResponseDto>(pageIndex, pageSize, totalCount, userRoles);

        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}