namespace F23.Kernel.Results;

/// <summary>
/// Represents the aggregation of multiple <see cref="Result"/> instances into a single result.
/// The overall success of the aggregate result is determined by the success state of all the underlying results.
/// </summary>
public class AggregateResult(IReadOnlyList<Result> results)
    : Result(results.All(r => r.IsSuccess))
{
    /// <summary>
    /// Gets the list of individual <see cref="Result"/> instances that are part of the aggregate result.
    /// </summary>
    public IReadOnlyList<Result> Results { get; } = results;

    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public override string Message => "Aggregate result of multiple results";
}
