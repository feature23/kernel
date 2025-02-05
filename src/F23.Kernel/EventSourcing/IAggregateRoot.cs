namespace F23.Kernel.EventSourcing;

/// <summary>
/// Represents the root interface for a data model aggregate.
/// Aggregates are clusters of domain objects that are treated as a single unit for data changes.
/// </summary>
public interface IAggregateRoot : IValidatable
{
    /// <summary>
    /// Gets the unique identifier of the aggregate root.
    /// </summary>
    string Id { get; }
}
