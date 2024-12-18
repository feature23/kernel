namespace F23.Kernel;

public interface IDomainEventDispatcher
{
    Task Dispatch<T>(T domainEvent) where T : IDomainEvent;
}
