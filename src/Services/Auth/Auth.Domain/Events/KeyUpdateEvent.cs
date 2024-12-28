

namespace Auth.Domain.Events;

public record KeyUpdateEvent(Key Key) : IDomainEvent;
