namespace F23.Kernel;

/// <summary>
/// A dispatcher that uses dependency injection to dispatch domain events to handlers resolved from an <see cref="IServiceProvider"/>.
/// </summary>
/// <param name="serviceProvider">The service provider used to resolve event handlers.</param>
public class DependencyInjectionDomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches the specified domain event to all registered handlers.
    /// </summary>
    /// <typeparam name="T">The type of the domain event.</typeparam>
    /// <param name="domainEvent">The domain event to dispatch.</param>
    /// <returns>A task that represents the asynchronous dispatch operation.</returns>
    public async Task Dispatch<T>(T domainEvent) where T : IDomainEvent
    {
        var handlers = serviceProvider.GetServices<IEventHandler<T>>();

        foreach (var handler in handlers)
        {
            await handler.Handle(domainEvent);
        }
    }
}
