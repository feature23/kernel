using F23.Kernel.Results;

namespace F23.Kernel.EventSourcing;

/// <summary>
/// A validator used to perform validation on an event within the context of an event stream.
/// </summary>
/// <typeparam name="T">The type of the aggregate root associated with the event stream.</typeparam>
/// <typeparam name="TEvent">The type of the event being validated.</typeparam>
public interface IEventValidator<T, in TEvent>
    where T : IAggregateRoot
    where TEvent : IEvent
{
    /// <summary>
    /// Validates the given event within the context of the specified event stream.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root associated with the event stream.</typeparam>
    /// <typeparam name="TEvent">The type of the event to validate.</typeparam>
    /// <param name="eventStream">The event stream containing the aggregate and its events.</param>
    /// <param name="e">The event to validate.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ValidationResult"/> representing the outcome of the validation.</returns>
    Task<ValidationResult> Validate(EventStream<T> eventStream, TEvent e, CancellationToken cancellationToken = default);
}
