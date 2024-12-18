using F23.Kernel.Results;

namespace F23.Kernel.EventSourcing;

public interface IEventValidator<T, in TEvent>
    where T : IAggregateRoot
    where TEvent : IEvent
{
    Task<ValidationResult> Validate(EventStream<T> eventStream, TEvent e, CancellationToken cancellationToken = default);
}
