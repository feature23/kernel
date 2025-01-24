using F23.Kernel.EventSourcing;
using F23.Kernel.Results;

namespace F23.Kernel.Tests.EventSourcing.Mocks;

public class TestEventValidator(bool passed) : IEventValidator<TestAggregateRoot, TestEvent>
{
    public Task<ValidationResult> Validate(EventStream<TestAggregateRoot> eventStream, TestEvent e, CancellationToken cancellationToken = default)
    {
        var result = passed
            ? ValidationResult.Passed()
            : ValidationResult.Failed(new List<ValidationError> { new("Test", "Test") });

        return Task.FromResult(result);
    }
}
