namespace F23.Kernel;

public class DependencyInjectionDomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    public async Task Dispatch<T>(T domainEvent) where T : IDomainEvent
    {
        var handlers = serviceProvider.GetServices<IEventHandler<T>>();

        foreach (var handler in handlers)
        {
            await handler.Handle(domainEvent);
        }
    }
}
