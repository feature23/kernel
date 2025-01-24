using F23.Kernel.EventSourcing;
using F23.Kernel.Results;

namespace F23.Kernel.Tests.EventSourcing.Mocks;

public class TestEvent(bool isValid = true) : IEvent
{
    public ValidationResult Validate() =>
        isValid ? ValidationResult.Passed() : ValidationResult.Failed("test", "test");

    public string EventType => "TestEvent";

    public string? UserProfileId { get; set; }

    public DateTime OccurredAt { get; set; }
}
