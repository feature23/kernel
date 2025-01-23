using Microsoft.Extensions.DependencyInjection;

namespace F23.Kernel.Tests;

public class KernelServiceCollectionExtensionsTests
{
    [Fact]
    public void RegisterQueryHandler_RegistersQueryHandler()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.RegisterQueryHandler<TestQuery, TestQueryResult, TestQueryHandler>();

        // Assert
        var provider = services.BuildServiceProvider();
        var validator = provider.GetRequiredService<IValidator<TestQuery>>();
        var handler = provider.GetRequiredService<IQueryHandler<TestQuery, TestQueryResult>>();
        Assert.IsType<DataAnnotationsValidator<TestQuery>>(validator);
        Assert.IsType<TestQueryHandler>(handler);
    }

    [Fact]
    public void RegisterCommandHandler_WithResult_RegistersCommandHandlerWithResult()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.RegisterCommandHandler<TestWithResultCommand, TestCommandResult, TestCommandHandlerWithResult>();

        // Assert
        var provider = services.BuildServiceProvider();
        var validator = provider.GetRequiredService<IValidator<TestWithResultCommand>>();
        var handler = provider.GetRequiredService<ICommandHandler<TestWithResultCommand, TestCommandResult>>();
        Assert.IsType<DataAnnotationsValidator<TestWithResultCommand>>(validator);
        Assert.IsType<TestCommandHandlerWithResult>(handler);
    }

    [Fact]
    public void RegisterCommandHandler_RegistersCommandHandler()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.RegisterCommandHandler<TestCommand, TestCommandHandler>();

        // Assert
        var provider = services.BuildServiceProvider();
        var validator = provider.GetRequiredService<IValidator<TestCommand>>();
        var handler = provider.GetRequiredService<ICommandHandler<TestCommand>>();
        Assert.IsType<DataAnnotationsValidator<TestCommand>>(validator);
        Assert.IsType<TestCommandHandler>(handler);
    }

    [Fact]
    public void RegisterEventHandler_RegistersEventHandler()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.RegisterEventHandler<TestEvent, TestEventHandler>();

        // Assert
        var provider = services.BuildServiceProvider();
        var handler = provider.GetRequiredService<IEventHandler<TestEvent>>();
        Assert.IsType<TestEventHandler>(handler);
    }

    private class TestQuery : IQuery<TestQueryResult> { }

    private class TestQueryResult { }

    private class TestQueryHandler : IQueryHandler<TestQuery, TestQueryResult>
    {
        public Task<Result<TestQueryResult>> Handle(TestQuery query, CancellationToken cancellationToken) => throw new NotImplementedException();
    }

    private class TestCommand : ICommand { }

    private class TestWithResultCommand : ICommand<TestCommandResult> { }

    private class TestCommandResult { }

    private class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task<Result> Handle(TestCommand command, CancellationToken cancellationToken) => throw new NotImplementedException();
    }

    private class TestCommandHandlerWithResult : ICommandHandler<TestWithResultCommand, TestCommandResult>
    {
        public Task<Result<TestCommandResult>> Handle(TestWithResultCommand command, CancellationToken cancellationToken) => throw new NotImplementedException();
    }

    private class TestEvent : IDomainEvent { }

    private class TestEventHandler : IEventHandler<TestEvent>
    {
        public Task Handle(TestEvent @event, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
