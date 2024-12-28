
namespace Auth.Domain.Events;

public record KeyCreateEvent(Key Key) : IDomainEvent;
