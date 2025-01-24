using F23.Kernel.EventSourcing;
using F23.Kernel.Tests.EventSourcing.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace F23.Kernel.Tests.EventSourcing;

public class EventValidatorSwitcherTests
{
    [Fact]
    public async Task Validate_WithNoValidators_ReturnsPassedValidationResult()
    {
        // Arrange
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var switcher = new EventValidatorSwitcher(serviceProvider);
        var eventStream = TestEventStream.Create();
        var e = new TestEvent();

        // Act
        var result = await switcher.Validate(eventStream, e);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Validate_WithOneValidator_ReturnsCorrectValidationResult(bool passed)
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddTransient<IEventValidator<TestAggregateRoot, TestEvent>>(_ => new TestEventValidator(passed))
            .BuildServiceProvider();
        var switcher = new EventValidatorSwitcher(serviceProvider);
        var eventStream = TestEventStream.Create();
        var e = new TestEvent();

        // Act
        var result = await switcher.Validate(eventStream, e);

        // Assert
        Assert.Equal(passed, result.IsSuccess);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public async Task Validate_WithMultipleValidators_ReturnsCorrectValidationResult(bool passed1, bool passed2)
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddTransient<IEventValidator<TestAggregateRoot, TestEvent>>(_ => new TestEventValidator(passed1))
            .AddTransient<IEventValidator<TestAggregateRoot, TestEvent>>(_ => new TestEventValidator(passed2))
            .BuildServiceProvider();
        var switcher = new EventValidatorSwitcher(serviceProvider);
        var eventStream = TestEventStream.Create();
        var e = new TestEvent();

        // Act
        var result = await switcher.Validate(eventStream, e);

        // Assert
        Assert.Equal(passed1 && passed2, result.IsSuccess);
    }
}
