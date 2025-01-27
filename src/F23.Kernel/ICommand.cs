namespace F23.Kernel;

/// <summary>
/// Represents a command that does not return a result.
/// </summary>
public interface ICommand;

/// <summary>
/// Represents a command that returns a result.
/// </summary>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ICommand<TResult> : ICommand;
