using Microsoft.Extensions.DependencyInjection;

namespace F23.Kernel.Tests;

public class DependencyInjectionDomainEventDispatcherTests
{
    [Fact]
    public async Task Dispatch_WithHandlers_DispatchesToHandlers()
    {
        // Arrange
        var handler = new TestDomainEventHandler();
        var serviceProvider = new ServiceCollection()
            .AddTransient<IEventHandler<TestDomainEvent>>(sp => handler)
            .BuildServiceProvider();

        var dispatcher = new DependencyInjectionDomainEventDispatcher(serviceProvider);
        var domainEvent = new TestDomainEvent();

        // Act
        await dispatcher.Dispatch(domainEvent);

        // Assert
        Assert.Equal(1, handler.HandleCount);
    }

    private class TestDomainEvent : IDomainEvent;

    private class TestDomainEventHandler : IEventHandler<TestDomainEvent>
    {
        public int HandleCount { get; private set; }

        public Task Handle(TestDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            HandleCount++;
            return Task.CompletedTask;
        }
    }
}
