
using Auth.Application.DTOs.Key;
using Auth.Application.DTOs.Key.Requests;
using Auth.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Services;

public class AuthRepository
(UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IDistributedCache cache,
    IJwtTokenGenerator jwtTokenGenerator,
    IMapper mapper,
    IKeyRepository<Guid> keyRepository,
    IHttpContextAccessor accessor)
: IAuthRepository
{
    private static bool CheckKeyExpire(IEnumerable<KeyDto> keys)
    {

        if (keys?.Count() > 0)
        {
            var keyLast = keys.Last();
            if (keyLast.IsUsed == true || keyLast.IsRevoked == true || keyLast.Expire > DateTime.Now)
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        // find user
        var checkExitUser = await userManager.FindByEmailAsync(dto.Email);
        
        if(checkExitUser is null)
        {
            throw new NotFoundException($"Account with email: {dto.Email} is not exits");
        }

        var keys = await keyRepository.GetKeysByUserIdAsync(checkExitUser.Id);
        // check password is incorrect
        var isPasswordValid = await userManager.CheckPasswordAsync(checkExitUser, dto.Password);
        if (!isPasswordValid)
        {
            checkExitUser.LockoutEnabled = true;
            checkExitUser.AccessFailedCount++;
            if (checkExitUser.AccessFailedCount >= 5)
            {
                checkExitUser.LockoutEnd = DateTimeOffset.Now.AddMinutes(5);
            }

            await userManager.UpdateAsync(checkExitUser); 
            throw new BadRequestException("Password in not correct");
        }
        
        // check is confirm Email
        bool isConfirmEmail = await userManager.IsEmailConfirmedAsync(checkExitUser);
        if (!isConfirmEmail)
        {
            throw new BadRequestException($"Account with email {checkExitUser.Email} is not confirm");
        }

        bool isLocked = await userManager.IsLockedOutAsync(checkExitUser);
        if (isLocked)
        {
            throw new BadRequestException($"Account with email {checkExitUser.Email} is locked");
        }

        int userStatus = checkExitUser.Status;
        if (userStatus == 3)
        {
            throw new BadRequestException($"Account.....");
        }
        
        // get-all roles of user
        var roles = await userManager.GetRolesAsync(checkExitUser);
        string accessToken = "";
        var cacheToken = await cache.GetStringAsync($"token-{checkExitUser.Id}");
        if (string.IsNullOrEmpty(cacheToken))
        {
            accessToken = jwtTokenGenerator.GeneratorToken(checkExitUser, roles);
            await cache.SetStringAsync($"token-{checkExitUser.Id}", accessToken, new DistributedCacheEntryOptions{AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)});
        }
        else
        {
            accessToken = cacheToken;
        }

        bool isCheckKey = CheckKeyExpire(keys);
        string refreshToken = "";
        if (!isCheckKey)
        {
            refreshToken = jwtTokenGenerator.GeneratorRefreshToken(checkExitUser.Id);
            await keyRepository.CreateKeyAsync(new CreateKeyRequestDto
            (
                Token :refreshToken,
                UserId : checkExitUser.Id
            ));
        }
        else
        {
            refreshToken = keys.Last().Token;
        }
        var userDto = mapper.Map<UserDto>(checkExitUser);

        checkExitUser.AccessFailedCount = 0;
        await userManager.UpdateAsync(checkExitUser);
        return new LoginResponseDto(
            userDto, new LoginTokenResponseDto(accessToken, refreshToken));
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        
        var checkExitUser = await userManager.FindByEmailAsync(dto.Email);
        if (checkExitUser is not null)
        {
            throw new BadRequestException($"Account with email {dto.Email} is exits");
        }

        var user = new ApplicationUser
        {
            Email = dto.Email,
            UserName = dto.Email,
            FullName = dto.FullName,
            Status = 1,
            Avatar = ""
        };

        try
        {
            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors.FirstOrDefault()!.ToString());
            }

            var roles = await userManager.GetRolesAsync(user);
            var accessToken = jwtTokenGenerator.GeneratorToken(user, roles);
            var refreshToken = jwtTokenGenerator.GeneratorRefreshToken(user.Id.ToString());
            var response = new LoginResponseDto(new UserDto(user.Id.ToString(), user.UserName, user.FullName),
                new LoginTokenResponseDto(accessToken, refreshToken));
            return response;
        }
        catch (Exception exception)
        {
            throw new BadRequestException(exception.Message.ToString());
        }
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordRequestDto dto)
    {
        string token = "";
        string uid = "";
        if (accessor.HttpContext!.Request.Headers.TryGetValue("Authorization", out var accessToken))
        {
            token = accessToken.ToString().Split(" ").Last();
        }

        if (accessor.HttpContext!.Request.Headers.TryGetValue("x-client-id", out var userIdRequest))
        {
            uid = userIdRequest.ToString();
        }

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(uid))
        {
            throw new BadRequestException("Invalid Token or UserId");
        }

        try
        {
            UserDto userRequest = jwtTokenGenerator.DecodeToken(token);
            if (string.IsNullOrEmpty(userRequest.Id) || !string.Equals(userRequest.Id, uid))
            {
                throw new BadRequestException("Invalid User Request");
            }

            var userFound = await userManager.FindByIdAsync(uid);
            if (userFound is null)
            {
                throw new NotFoundException("User NotFound");
            }

            var isChangePassword = await userManager.ChangePasswordAsync(userFound, dto.OldPassword, dto.NewPassword);
            if (isChangePassword.Succeeded)
            {
                return true;
            }
            
            foreach (var error in isChangePassword.Errors)
            {
                Console.WriteLine(error.Description);
            }

            return false;
        }
        catch (SecurityTokenException ex)
        {
            // Log the exception
            Console.WriteLine($"Token validation error: {ex.Message}");
            throw new BadRequestException("Invalid Token");
        }
    }

    public async Task<bool> LockUserAsync(LockUserRequestDto dto)
    {
        try
        {
            if (
                accessor.HttpContext!.Request.Headers.TryGetValue("x-client-id", out var uid)
                && accessor.HttpContext!.Request.Headers.TryGetValue("Authorization", out var accessToken))
            {
                if (string.IsNullOrEmpty(uid.ToString()) || string.IsNullOrEmpty(accessToken.ToString()))
                {
                    throw new BadRequestException("Token Is Null");
                }
                UserDto userToken = jwtTokenGenerator.DecodeToken(accessToken!);
                if (string.IsNullOrEmpty(userToken.Id)) throw new BadRequestException("Invalid User");
                if (!string.Equals(uid.ToString(), userToken.Id)) throw new BadRequestException("Invalid UserId");
                var userFound = await userManager.FindByIdAsync(userToken.Id) ?? throw new NotFoundException("User NotFound");
                userFound.Status = (int)UserStatus.Locked;
                userFound.LockoutEnabled = true;
                userFound.LockoutEnd = DateTimeOffset.Now.AddDays(dto.Expire);
                var userUpdate = await userManager.UpdateAsync(userFound);
                if (!userUpdate.Succeeded)
                {
                    return false;
                }
                return true;
            }

            throw new BadRequestException("Invalid Token");
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> DeleteUserAsync(DeleteUserRequestDto dto)
    {
        try
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user is null)
            {
                return false;
            }

            user.Status = (int)UserStatus.Removed;
            IdentityResult userUpdate = await userManager.UpdateAsync(user);
            if (!userUpdate.Succeeded)
            {
                throw new BadRequestException(userUpdate.Errors.FirstOrDefault()!.Description);
            }

            return true;

        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}