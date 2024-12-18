namespace F23.Kernel;

public interface IEventHandler<in T>
    where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}
