using BuildingBlocks.Exceptions;

namespace Auth.Application.Exceptions;

public class KeyNotFoundException : NotFoundException
{
    public KeyNotFoundException(Guid id) : base($"Key {id}")
    {
        
    }
}