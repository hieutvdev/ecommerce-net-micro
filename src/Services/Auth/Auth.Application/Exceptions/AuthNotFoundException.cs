using BuildingBlocks.Exceptions;

namespace Auth.Application.Exceptions;

public class AuthNotFoundException : NotFoundException
{
    public AuthNotFoundException(Guid id) : base($"Auth {id}")
    {
        
    }
}