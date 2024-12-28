using Auth.Application.DTOs.Key;
using Auth.Application.DTOs.Key.Requests;

namespace Auth.Application.Auths.Commands.CreateKey;

public record CreateKeyCommand(CreateKeyRequestDto Request) : ICommand<CreateKeyResult>;

public record CreateKeyResult(Guid Id);
