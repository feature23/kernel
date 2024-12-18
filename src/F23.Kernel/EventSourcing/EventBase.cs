using F23.Kernel.Results;

namespace F23.Kernel.EventSourcing;

public abstract record EventBase : IEvent
{
    public string EventType => GetType().Name;

    public string? UserProfileId { get; set; }

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public abstract ValidationResult Validate();
}
