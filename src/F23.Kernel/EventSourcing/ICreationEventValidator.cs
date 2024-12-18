namespace F23.Kernel.EventSourcing;

public interface ICreationEventValidator<in TEvent> : IValidator<TEvent>
    where TEvent : ICreationEvent;
