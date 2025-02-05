using System.Linq.Expressions;

namespace F23.Kernel.EventSourcing;

/// <summary>
/// Represents a repository for managing event streams associated with aggregate roots.
/// Provides read/write operations to manage event streams and query capabilities for snapshots.
/// Requires a generic type parameter that implements <see cref="IAggregateRoot"/>.
/// </summary>
/// <typeparam name="T">The type of the aggregate root associated with the event streams.</typeparam>
public interface IEventStreamRepo<T>
    where T : IAggregateRoot
{
    /// <summary>
    /// Adds a new event stream to the repository.
    /// </summary>
    /// <param name="stream">The event stream to be added, containing the aggregate root's snapshot and its events.</param>
    /// <param name="userId">The identifier of the user performing the operation. Optional parameter.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the request. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddNewEventStream(EventStream<T> stream, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits an event stream by marking all uncommitted events in the stream as committed and persisting them to the data store.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root associated with the event stream.</typeparam>
    /// <param name="stream">The event stream containing the uncommitted events to be committed.</param>
    /// <param name="userId">The identifier of the user performing the operation. This parameter is optional.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CommitEventStream(EventStream<T> stream, string? userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves only the snapshot of an event stream identified by the given id.
    /// </summary>
    /// <param name="id">The unique identifier of the event stream to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="EventStream{T}"/>
    /// representing the snapshot of the event stream, or null if the event stream does not exist.
    /// </returns>
    Task<EventStream<T>?> GetSnapshotOnly(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the full event stream, including committed events, for a specified aggregate root by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the aggregate root for which the event stream is being retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the event stream associated with the specified identifier,
    /// or null if the event stream does not exist.
    /// </returns>
    Task<EventStream<T>?> GetFullEventStream(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Queries snapshots of the aggregate root type based on the given predicate and optional pagination parameters.
    /// </summary>
    /// <param name="predicate">An expression that defines the condition to filter the snapshots.</param>
    /// <param name="pageNumber">The optional number of the page to retrieve. If null, no pagination is applied.</param>
    /// <param name="pageSize">The optional number of items per page. If null, no pagination is applied.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a read-only list of snapshots
    /// that match the provided predicate and pagination criteria.
    /// </returns>
    Task<IReadOnlyList<T>> QuerySnapshots(Expression<Func<T, bool>> predicate,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Queries a collection of snapshots of the aggregate root type, applying the specified filter,
    /// sorting, and pagination parameters.
    /// </summary>
    /// <typeparam name="TOrderBy">The type used for ordering the query result.</typeparam>
    /// <param name="predicate">An expression defining the filter to apply to the query.</param>
    /// <param name="orderBy">An expression specifying the property to order the results by.</param>
    /// <param name="descending">A boolean value determining the sort order. If true, results will be ordered descending; otherwise, ascending.</param>
    /// <param name="pageNumber">The page number to retrieve, used for pagination. Null indicates no pagination.</param>
    /// <param name="pageSize">The size of the page to retrieve, used for pagination. Null indicates no pagination.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of snapshots matching the filter, sorted, and paginated as specified.</returns>
    Task<IReadOnlyList<T>> QuerySnapshots<TOrderBy>(Expression<Func<T, bool>> predicate,
        Expression<Func<T, TOrderBy>> orderBy,
        bool descending = false,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of snapshots that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">A query expression used to filter snapshots based on specified criteria.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the total count of snapshots matching the specified predicate.
    /// </returns>
    Task<int> CountSnapshots(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all events associated with a specific event stream by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the event stream.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of events associated with the specified event stream.</returns>
    Task<IReadOnlyList<IEvent>> GetEventsForStream(string id, CancellationToken cancellationToken = default);
}
