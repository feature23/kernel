using F23.Kernel.EventSourcing;
using F23.Kernel.Results;
using F23.Kernel.Tests.EventSourcing.Mocks;

namespace F23.Kernel.Tests.EventSourcing;

public class EventStreamTests
{
    [Fact]
    public void EventStream_Ctor_Snapshot()
    {
        // Arrange
        var snapshot = new TestAggregateRoot();

        // Act
        var eventStream = new TestEventStream<TestAggregateRoot>(snapshot);

        // Assert
        Assert.Equal(snapshot, eventStream.Snapshot);
        Assert.Equal(snapshot, eventStream.LastCommittedSnapshot);
        Assert.Equal(snapshot.Id, eventStream.Id);
        Assert.Empty(eventStream.CommittedEvents);
        Assert.Empty(eventStream.UncommittedEvents);
        Assert.Empty(eventStream.AllEvents);
    }

    [Fact]
    public void EventStream_Ctor_SnapshotAndEvents()
    {
        // Arrange
        var snapshot = new TestAggregateRoot();
        var events = new List<IEvent>
        {
            new TestEvent()
        };

        // Act
        var eventStream = new TestEventStream<TestAggregateRoot>(snapshot, events);

        // Assert
        Assert.Equal(snapshot, eventStream.Snapshot);
        Assert.Equal(snapshot, eventStream.LastCommittedSnapshot);
        Assert.Equal(snapshot.Id, eventStream.Id);
        Assert.Equal(events, eventStream.CommittedEvents);
        Assert.Empty(eventStream.UncommittedEvents);
        Assert.Equal(events, eventStream.AllEvents);
    }

    [Fact]
    public void EventStream_Apply_CreationEvent_Throws()
    {
        // Arrange
        var snapshot = new TestAggregateRoot();
        var eventStream = new TestEventStream<TestAggregateRoot>(snapshot);
        var creationEvent = new TestCreationEvent();

        // Act
        void Act() => eventStream.Apply(creationEvent);

        // Assert
        Assert.Throws<InvalidOperationException>(Act);
    }

    [Fact]
    public void EventStream_Apply_ValidEvent()
    {
        // Arrange
        var snapshot = new TestAggregateRoot();
        var eventStream = new TestEventStream<TestAggregateRoot>(snapshot, [new TestCreationEvent()]);
        var validEvent = new TestEvent();

        // Act
        eventStream.Apply(validEvent);

        // Assert
        Assert.Equal(snapshot, eventStream.Snapshot);
        Assert.Equal(snapshot, eventStream.LastCommittedSnapshot);
        Assert.Equal(snapshot.Id, eventStream.Id);
        Assert.Single(eventStream.CommittedEvents);
        Assert.Single(eventStream.UncommittedEvents);
        Assert.Equal(2, eventStream.AllEvents.Count());
    }

    [Fact]
    public void EventStream_Commit()
    {
        // Arrange
        var snapshot = new TestAggregateRoot();
        var eventStream = new TestEventStream<TestAggregateRoot>(snapshot, [new TestCreationEvent()]);
        var validEvent = new TestEvent();
        eventStream.Apply(validEvent);

        // Act
        eventStream.Commit();

        // Assert
        Assert.Equal(snapshot, eventStream.Snapshot);
        Assert.NotSame(snapshot, eventStream.Snapshot);
        Assert.Equal(snapshot, eventStream.LastCommittedSnapshot);
        Assert.NotSame(snapshot, eventStream.LastCommittedSnapshot);
        Assert.Equal(snapshot.Id, eventStream.Id);
        Assert.Equal(2, eventStream.CommittedEvents.Count);
        Assert.Empty(eventStream.UncommittedEvents);
        Assert.Equal(2, eventStream.AllEvents.Count());
    }

    [Fact]
    public void EventStream_Rollback()
    {
        // Arrange
        var snapshot = new TestAggregateRoot();
        var eventStream = new TestEventStream<TestAggregateRoot>(snapshot, [new TestCreationEvent()]);
        var validEvent = new TestEvent();
        eventStream.Apply(validEvent);

        // Act
        eventStream.Rollback();

        // Assert
        Assert.Same(snapshot, eventStream.Snapshot);
        Assert.Same(snapshot, eventStream.LastCommittedSnapshot);
        Assert.Equal(snapshot.Id, eventStream.Id);
        Assert.Single(eventStream.CommittedEvents);
        Assert.Empty(eventStream.UncommittedEvents);
        Assert.Single(eventStream.AllEvents);
    }

    private class TestEventStream<T> : EventStream<T>
        where T : IAggregateRoot
    {
        public TestEventStream(T snapshot) : base(snapshot)
        {
        }

        public TestEventStream(T snapshot, IEnumerable<IEvent> events) : base(snapshot, events)
        {
        }

        public T LastCommittedSnapshot => LastCommittedSnapshot_FOR_UNIT_TESTING;
    }
}
