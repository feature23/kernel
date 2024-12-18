using System.Linq.Expressions;

namespace F23.Kernel.EventSourcing;

public interface IEventStreamRepo<T>
    where T : IAggregateRoot
{
    Task AddNewEventStream(EventStream<T> stream, string? userId, CancellationToken cancellationToken = default);

    Task CommitEventStream(EventStream<T> stream, string? userId, CancellationToken cancellationToken = default);

    Task<EventStream<T>?> GetSnapshotOnly(string id, CancellationToken cancellationToken = default);

    Task<EventStream<T>?> GetFullEventStream(string id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> QuerySnapshots(Expression<Func<T, bool>> predicate,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> QuerySnapshots<TOrderBy>(Expression<Func<T, bool>> predicate,
        Expression<Func<T, TOrderBy>> orderBy,
        bool descending = false,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default);

    Task<int> CountSnapshots(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<IEvent>> GetEventsForStream(string id, CancellationToken cancellationToken = default);
}
