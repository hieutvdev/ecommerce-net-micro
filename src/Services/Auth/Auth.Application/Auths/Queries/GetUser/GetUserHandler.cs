using Auth.Application.DTOs.Users.Requests;
using Auth.Application.DTOs.Users.Responses;

namespace Auth.Application.Auths.Queries.GetUser;

public class GetUserHandler
(IUserRepository userRepository)
: IQueryHandler<GetUserQuery, GetUserResult>
{
    public async Task<GetUserResult> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var getUserRequestDto = UserQueryToUserRequestDto(query);
        var result = await userRepository.GetUserAsync(getUserRequestDto);
        return UserResponseDtoToUserResult(result);
    }


    private static GetUserRequestDto UserQueryToUserRequestDto(GetUserQuery query)
        => new GetUserRequestDto(query.UserId);

    private static GetUserResult UserResponseDtoToUserResult(GetUserResponseDto dto)
        => new GetUserResult(
            dto.UserId,
            dto.Email,
            dto.Username,
            dto.FullName,
            dto.PhoneNumber,
            dto.Avatar);
}