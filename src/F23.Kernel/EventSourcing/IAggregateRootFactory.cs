namespace F23.Kernel.EventSourcing;

/// <summary>
/// Represents a factory interface for creating instances of aggregate roots.
/// Aggregate roots are the primary entry points for interactions with aggregates,
/// which are clusters of domain objects treated as a single unit according to
/// business logic.
/// </summary>
/// <typeparam name="T">
/// The type of the aggregate root to be created. Must implement <see cref="IAggregateRoot"/>.
/// </typeparam>
/// <typeparam name="TCreationEvent">
/// The type of the creation event used for initializing the aggregate root. Must
/// implement <see cref="ICreationEvent"/>.
/// </typeparam>
public interface IAggregateRootFactory<T, in TCreationEvent>
    where T : IAggregateRoot
    where TCreationEvent : ICreationEvent
{
    /// <summary>
    /// Creates an aggregate root instance of type <typeparamref name="T"/> based on the specified creation event.
    /// </summary>
    /// <param name="creationEvent">The event that contains the information needed to create the aggregate root.</param>
    /// <param name="cancellationToken">An optional cancellation token to observe during the asynchronous creation process.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/>
    /// which is either a successful result containing the created aggregate root instance or a failure result.
    /// </returns>
    Task<Result<T>> Create(TCreationEvent creationEvent, CancellationToken cancellationToken = default);
}
