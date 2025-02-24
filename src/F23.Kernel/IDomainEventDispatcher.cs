namespace F23.Kernel;

/// <summary>
/// Defines a dispatcher for domain events.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches the specified domain event to its handlers.
    /// </summary>
    /// <typeparam name="T">The type of the domain event.</typeparam>
    /// <param name="domainEvent">The domain event to dispatch.</param>
    /// <returns>A task that represents the asynchronous dispatch operation.</returns>
    Task Dispatch<T>(T domainEvent) where T : IDomainEvent;
}
