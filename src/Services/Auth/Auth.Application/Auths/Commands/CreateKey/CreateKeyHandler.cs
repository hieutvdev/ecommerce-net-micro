using Auth.Application.DTOs.Key.Requests;

namespace Auth.Application.Auths.Commands.CreateKey;

public class CreateKeyHandler
    (IKeyRepository<Guid> repository, IMapper mapper)
    : ICommandHandler<CreateKeyCommand, CreateKeyResult>
{
    public async Task<CreateKeyResult> Handle(CreateKeyCommand command, CancellationToken cancellationToken)
    {
        var key = mapper.Map<CreateKeyRequestDto>(command);
        var result = await repository.CreateKeyAsync(key, cancellationToken);
        return new CreateKeyResult(result);
    }
}