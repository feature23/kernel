using F23.Kernel.EventSourcing;
using F23.Kernel.Results;

namespace F23.Kernel.Tests.EventSourcing.Mocks;

public record TestAggregateRoot :
    IAggregateRoot,
    IApplyEvent<TestAggregateRoot, TestEvent>
{
    private readonly bool _isValid;

    public TestAggregateRoot(bool isValid = true)
    {
        _isValid = isValid;
    }

    public string Id => "test-id";

    public ValidationResult Validate() =>
        _isValid ? ValidationResult.Passed() : ValidationResult.Failed("test", "test");

    public TestAggregateRoot Apply(TestEvent e) => this with {};
}
