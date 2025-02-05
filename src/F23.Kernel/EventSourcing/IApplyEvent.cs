namespace F23.Kernel.EventSourcing;

/// <summary>
/// Defines a contract for a type that can apply a specific event and produce a new aggregate root state.
/// </summary>
/// <typeparam name="TSnapshot">
/// The type of the aggregate root to which the event is applied.
/// </typeparam>
/// <typeparam name="TEvent">
/// The type of the event to be applied to the aggregate root.
/// </typeparam>
/// <remarks>
/// While not enforceable by the type system, this interface is intended to be implemented by
/// aggregate root types, so that they can transform their current state into new state based on the event.
/// </remarks>
public interface IApplyEvent<out TSnapshot, in TEvent>
    where TEvent : IEvent
    where TSnapshot : IAggregateRoot
{
    /// <summary>
    /// Applies the specified event to the aggregate root and updates its state accordingly.
    /// </summary>
    /// <param name="e">The event to be applied to the aggregate root.</param>
    [UsedImplicitly]
    TSnapshot Apply(TEvent e);
}
