namespace Auth.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base($"Domain Exception: \"{message}\" throw from Domain Layer!")
    {
        
    }
}