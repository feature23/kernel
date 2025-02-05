namespace F23.Kernel.EventSourcing;

/// <summary>
/// Represents an event that is used to create an aggregate root.
/// </summary>
/// <remarks>
/// This interface is a marker for creation events within the event-sourcing pattern.
/// It should be used for defining events that establish the initial state of an aggregate root.
/// </remarks>
public interface ICreationEvent : IEvent;
