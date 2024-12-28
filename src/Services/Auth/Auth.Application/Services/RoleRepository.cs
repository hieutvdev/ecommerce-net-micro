using Auth.Application.DTOs.Roles.Requests;
using Auth.Application.DTOs.Roles.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Services;

public class RoleRepository(
    RoleManager<IdentityRole> roleManager,
    IDistributedCache cache,
    ILogger<RoleRepository> logger,
    UserManager<ApplicationUser> userManager)
    : IRoleRepository
{

    public async Task<IEnumerable<object>> GetRolesAsync(CancellationToken cancellationToken = default!)
    {
        try
        {
            var roles = await roleManager.Roles.ToListAsync(cancellationToken);
            List<object> responses = [];
            foreach (var role in roles)
            {
                var userWithRole = await userManager.GetUsersInRoleAsync(role.Name!);
                var totalUser = userWithRole.Count();
                var roleResponse = new
                {
                    RoleId = role.Id,
                    RoleName = role.Name!,
                    TotalUser = totalUser
                };
                responses.Add(roleResponse);
               
            }
            return responses;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> AssignRolesAsync(AssignRoleRequestDto dto, CancellationToken cancellationToken = default!)
    {
        try
        {
            foreach (var roleName in dto.RoleNames)
            {
                var roleIsExits = await roleManager.RoleExistsAsync(roleName);
                if (!roleIsExits)
                {
                    var roleCreate = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleCreate.Succeeded)
                    {
                        throw new BadRequestException("AssignRole Error");
                    }
                }
            }

            var userFound = await userManager.FindByEmailAsync(dto.Email) ??
                            throw new NotFoundException("User NotFound");
            var assignRoleForUser = await userManager.AddToRolesAsync(userFound, dto.RoleNames);
            if (!assignRoleForUser.Succeeded)
            {
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> DeleteRoleAsync(DeleteRoleRequestDto dto, CancellationToken cancellationToken = default!)
    {
        try
        {
            var roleFound = await roleManager.FindByNameAsync(dto.RoleName) ??
                            throw new NotFoundException("Role NotFound");
            var isDeleted = await roleManager.DeleteAsync(roleFound);
            if (!isDeleted.Succeeded) return false;
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateRoleAsync(UpdateRoleRequestDto dto, CancellationToken cancellationToken = default!)
    {
        try
        {
            var roleFound = await roleManager.FindByIdAsync(dto.RoleId) ?? throw new NotFoundException("Role NotFound");
            roleFound.Name = dto.RoleName;
            roleFound.NormalizedName = dto.RoleName.ToUpper();
            var updateRole = await roleManager.UpdateAsync(roleFound);
            if (!updateRole.Succeeded) return false;
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> CreateRoleAsync(CreateRoleRequestDto dto, CancellationToken cancellationToken = default!)
    {
        try
        {
            var roleFound = await roleManager.RoleExistsAsync(dto.RoleName);
            if (roleFound)
            {
                throw new BadRequestException("Role already exists");
            }

            var roleCreate = await roleManager.CreateAsync(new IdentityRole(dto.RoleName));
            if (!roleCreate.Succeeded)
            {
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BadRequestException(e.Message);
        }
    }
}