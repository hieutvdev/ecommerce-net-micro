using Auth.Domain.Abstractions;
using Auth.Domain.Events;
using BuildingBlocks.Abstractions;

namespace Auth.Domain.Models;

public class Key : Aggregate<KeyId>
{
    public string Token { get; private set; } = default!;
    public string UserId { get; private set; } = default!;
    public DateTime Expires { get; private set; }
    public bool IsUsed { get; private set; }
    public bool IsRevoked { get; private set; }

    public static Key Create(string token, string userId, DateTime expires, bool isUsed, bool isRevoked)
    {
        var key = new Key
        {
            Id = KeyId.Of(Guid.NewGuid()),
            Token = token,
            UserId = userId,
            Expires = expires,
            IsUsed = isUsed,
            IsRevoked = isRevoked,
        };
        key.AddDomainEvent(new KeyCreateEvent(key));
        return key;
    }

    public void Update(DateTime expires, bool isUsed, bool isRevoked)
    {
        Expires = expires;
        IsRevoked = isRevoked;
        IsUsed = isUsed;
        
        AddDomainEvent(new KeyUpdateEvent(this));
    }
}