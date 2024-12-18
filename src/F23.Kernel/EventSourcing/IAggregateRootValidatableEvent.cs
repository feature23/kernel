using F23.Kernel.Results;

namespace F23.Kernel.EventSourcing;

public interface IAggregateRootValidatableEvent<in TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    ValidationResult Validate(TAggregateRoot aggregateRoot);
}
