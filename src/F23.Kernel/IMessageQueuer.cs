namespace F23.Kernel;

/// <summary>
/// Defines a queuer for messages without regard for underlying transport mechanisms.
/// </summary>
/// <typeparam name="TMessage">The type of the message.</typeparam>
public interface IMessageQueuer<in TMessage>
    where TMessage : IMessage
{
    /// <summary>
    /// Enqueues the specified message.
    /// </summary>
    /// <param name="message">The message to enqueue.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous enqueue operation.</returns>
    Task Enqueue(TMessage message, CancellationToken cancellationToken = default);
}
