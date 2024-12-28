using Auth.Application.DTOs.Users.Requests;
using Auth.Application.DTOs.Users.Responses;

namespace Auth.Application.Auths.Commands.EditInForUser;

public class EditInForUserHandler
(IUserRepository repository)
: ICommandHandler<EditInForUserCommand, EditInForUserResult>
{
    public async Task<EditInForUserResult> Handle(EditInForUserCommand request, CancellationToken cancellationToken)
    {
        var editInForUserRequest = EditInForCommandToDto(request);
        var result = await repository.EditInForUserAsync(editInForUserRequest);
        return EditUserResponseToUserResult(result);
    }

    private static EditInForUserRequestDto EditInForCommandToDto(EditInForUserCommand command)
        => new EditInForUserRequestDto(command.UserId, command.FullName, command.PhoneNumber, command.Avatar);

    private static EditInForUserResult EditUserResponseToUserResult(GetUserResponseDto dto)
        => new EditInForUserResult(dto.UserId, dto.Email, dto.Username, dto.FullName, dto.PhoneNumber, dto.Avatar);
}