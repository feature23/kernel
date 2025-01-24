using F23.Kernel.EventSourcing;

namespace F23.Kernel.Tests.EventSourcing.Mocks;

public static class TestEventStream
{
    public static EventStream<TestAggregateRoot> Create() =>
        new(new TestAggregateRoot(), [new TestCreationEvent()]);
}
