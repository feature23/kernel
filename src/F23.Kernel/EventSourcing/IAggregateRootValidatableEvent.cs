using F23.Kernel.Results;

namespace F23.Kernel.EventSourcing;

/// <summary>
/// Represents an event that can be validated against a specific state of an aggregate root.
/// </summary>
/// <typeparam name="TAggregateRoot">The type of the aggregate root against which the event is validated. It must implement <see cref="IAggregateRoot"/>.</typeparam>
public interface IAggregateRootValidatableEvent<in TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    /// <summary>
    /// Validates the provided aggregate root and returns a validation result.
    /// </summary>
    /// <param name="aggregateRoot">The aggregate root instance to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the validation passed or failed.</returns>
    ValidationResult Validate(TAggregateRoot aggregateRoot);
}
