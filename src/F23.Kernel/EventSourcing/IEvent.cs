namespace F23.Kernel.EventSourcing;

public interface IEvent : IDomainEvent, IValidatable
{
    string EventType { get; }

    string? UserProfileId { get; set; }

    DateTime OccurredAt { get; set; }
}
