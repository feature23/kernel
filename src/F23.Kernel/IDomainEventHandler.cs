namespace F23.Kernel;

/// <summary>
/// Defines a handler for a domain event.
/// </summary>
/// <typeparam name="T">The type of the domain event.</typeparam>
public interface IEventHandler<in T>
    where T : IDomainEvent
{
    /// <summary>
    /// Handles the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handle operation.</returns>
    Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}
