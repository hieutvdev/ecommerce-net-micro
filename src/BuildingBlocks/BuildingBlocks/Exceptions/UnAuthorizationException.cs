namespace BuildingBlocks.Exceptions;

public class UnAuthorizationException : Exception
{
    public UnAuthorizationException(string message) : base(message)
    {
        
    }

    public UnAuthorizationException(string message, string details) : base(message)
    {
        Details = details;
    }

    public string? Details;

}