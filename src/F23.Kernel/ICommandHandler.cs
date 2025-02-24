namespace F23.Kernel;

/// <summary>
/// Defines a handler for a command that does not return a result.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Handles the specified command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handle operation.</returns>
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines a handler for a command that returns a result.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Handles the specified command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous handle operation. The task result contains the result of the command.</returns>
    Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken = default);
}
