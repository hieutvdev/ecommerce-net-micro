using System.Text.Json.Serialization;
using Auth.Application.Data;
using Auth.Application.DTOs.Key;
using Auth.Application.DTOs.Key.Requests;
using Auth.Application.DTOs.Key.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Auth.Application.Services;

public class KeyRepository(
    IApplicationDbContext context,
    IJwtTokenGenerator jwtTokenGenerator,
    IMapper mapper,
    IHttpContextAccessor contextAccessor,
    IDistributedCache cache,
    UserManager<ApplicationUser> userManager)
    : IKeyRepository<Guid>
{
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> CreateKeyAsync(CreateKeyRequestDto dto, CancellationToken cancellationToken)
    {
        try
        {

            var token = jwtTokenGenerator.GeneratorRefreshToken(dto.UserId);
            var keyModel = Key.Create(
                token: token,
                userId: dto.UserId,
                expires: DateTime.UtcNow.AddDays(7),
                isUsed: false,
                isRevoked: false);
            context.Keys.Add(keyModel);
            bool isCreated =  await context.SaveChangesAsync(cancellationToken) > 0;
            if (!isCreated)
            {
                throw new BadRequestException("Create key is failure");
            }
            return keyModel.Id.Value;
        }
        catch (Exception exception)
        {
            throw new BadRequestException(exception.Message);
        }
    }

    public async Task<IEnumerable<KeyDto>> GetKeysByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new BadRequestException("User is required");
            }
            var keyByUsers = await context.Keys
                .AsTracking()
                .Where(c => c.UserId == userId)
                .ToListAsync(cancellationToken: cancellationToken);

            if (keyByUsers.Count > 0)
            {
                IEnumerable<KeyDto> keyDtos = keyByUsers
                    .Select(k => new KeyDto(k.UserId, k.Token, k.Expires, k.IsUsed, k.IsRevoked)).ToList();
                return keyDtos;
            }

            return null;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<RefreshTokenByUserResponseDto> RefreshTokenByUser(RefreshTokenByUserRequestDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            string userId = "";
            if(contextAccessor.HttpContext!.Request.Headers.TryGetValue("x-client-id", out var clientId)){
                userId = Convert.ToString(clientId);
            }
            
            if (string.IsNullOrEmpty(userId))
            {
                throw new BadRequestException("Invalid token");
            }

            var userFound = await userManager.FindByIdAsync(userId);
            if (userFound is null)
            {
                throw new BadRequestException("Invalid User");
            }

            var roles = await userManager.GetRolesAsync(userFound);
            var token = await context.Keys.AsTracking().FirstOrDefaultAsync(t => t.Token == dto.Token && t.UserId == userId, cancellationToken);
            if (token is null)
            {
                throw new BadRequestException("Token is null");
            }

            var checkToken = token.Expires > DateTime.UtcNow && token is { IsRevoked: false, IsUsed: false };
            if (checkToken)
            {
                context.Keys.Remove(token);
                await cache.RemoveAsync($"token-{userId}", cancellationToken);
                var accessToken = jwtTokenGenerator.GeneratorToken(userFound, roles);
                var refreshToken = jwtTokenGenerator.GeneratorRefreshToken(userId);

                CreateKeyRequestDto requestDto = new CreateKeyRequestDto(refreshToken, userId);

                var keyCreate = await CreateKeyAsync(requestDto, cancellationToken);
                if (!string.IsNullOrEmpty(keyCreate.ToString()))
                {
                    await cache.SetStringAsync($"token-{userId}", accessToken, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                    }, cancellationToken);
                    return new RefreshTokenByUserResponseDto(accessToken, refreshToken);
                }

                throw new BadRequestException("Create refresh Token Failure");
            }

            throw new BadRequestException("Token is expired");
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<KeyDto>> GetKeysAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            int pageIndex = paginationRequest.PageIndex;
            int pageSize = paginationRequest.PageSize;
            long keyCount = 0;

            var cacheData = await cache.GetStringAsync("keys-list", cancellationToken);
           
            // if (!string.IsNullOrEmpty(cacheData))
            // {
            //     
            //     keyCount = JsonConvert.DeserializeObject<IEnumerable<KeyDto>>(cacheData)!.LongCount();
            //     
            // }
            keyCount = await context.Keys.AsNoTracking().LongCountAsync(cancellationToken);

            var keys = await context.Keys.AsNoTracking().Skip(pageIndex * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken);
            
            var keysDto = KeyExtensions.KeyToDto(keys);
            PaginatedResult<KeyDto> result = new PaginatedResult<KeyDto>(pageIndex, pageSize, keyCount, keysDto);
            return result;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}