namespace F23.Kernel;

/// <summary>
/// Defines a handler for a query that returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    /// <summary>
    /// Handles the specified query.
    /// </summary>
    /// <param name="query">The query to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handle operation, containing the result of type <typeparamref name="TResult"/>.</returns>
    Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default);
}
