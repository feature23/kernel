using F23.Kernel.EventSourcing;
using F23.Kernel.Results;

namespace F23.Kernel.Tests.EventSourcing.Mocks;

public class TestCreationEvent(bool isValid = true) : ICreationEvent
{
    public ValidationResult Validate() =>
        isValid ? ValidationResult.Passed() : ValidationResult.Failed("test", "test");

    public string EventType => "TestCreationEvent";

    public string? UserProfileId { get; set; }

    public DateTime OccurredAt { get; set; }
}
