namespace F23.Kernel;

public interface IMessageQueuer<in TMessage>
    where TMessage : IMessage
{
    Task Enqueue(TMessage message, CancellationToken cancellationToken = default);
}
