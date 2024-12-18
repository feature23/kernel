namespace F23.Kernel.EventSourcing;

public interface IAggregateRoot : IValidatable
{
    string Id { get; }
}
