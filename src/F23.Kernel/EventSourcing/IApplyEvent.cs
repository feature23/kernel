namespace F23.Kernel.EventSourcing;

public interface IApplyEvent<out TSnapshot, in TEvent>
    where TEvent : IEvent
    where TSnapshot : IAggregateRoot
{
    [UsedImplicitly]
    TSnapshot Apply(TEvent e);
}
