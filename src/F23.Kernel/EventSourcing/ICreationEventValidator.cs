namespace F23.Kernel.EventSourcing;

/// <summary>
/// Defines the contract for a validator that validates events used to create aggregate roots.
/// </summary>
/// <typeparam name="TEvent">The type of the creation event to be validated. Must implement <see cref="ICreationEvent"/>.</typeparam>
public interface ICreationEventValidator<in TEvent> : IValidator<TEvent>
    where TEvent : ICreationEvent;
