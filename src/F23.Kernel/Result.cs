using F23.Kernel.Results;
using Microsoft.Extensions.Logging;

namespace F23.Kernel;

/// <summary>
/// Abstract representation of the result of an operation that does not have an associated value.
/// </summary>
public abstract class Result(bool isSuccess)
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; } = isSuccess;

    /// <summary>
    /// Gets a message describing the outcome of the operation.
    /// </summary>
    public abstract string Message { get; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A <see cref="SuccessResult"/>.</returns>
    public static Result Success()
        => new SuccessResult();

    /// <summary>
    /// Aggregates multiple results into a single result.
    /// </summary>
    /// <param name="results">The results to aggregate.</param>
    /// <returns>An <see cref="AggregateResult"/>.</returns>
    public static Result Aggregate(IReadOnlyList<Result> results)
        => new AggregateResult(results);

    /// <summary>
    /// Creates an unauthorized result.
    /// </summary>
    /// <param name="message">The unauthorized message.</param>
    /// <returns>An <see cref="UnauthorizedResult"/>.</returns>
    public static Result Unauthorized(string message)
        => new UnauthorizedResult(message);

    /// <summary>
    /// Creates a validation failed result for a collection of <see cref="ValidationError"/> objects.
    /// </summary>
    /// <param name="errors">The validation errors.</param>
    /// <returns>A <see cref="ValidationFailedResult"/>.</returns>
    public static Result ValidationFailed(IReadOnlyCollection<ValidationError> errors)
        => new ValidationFailedResult(errors);

    /// <summary>
    /// Creates a validation failed result for a single key and message.
    /// </summary>
    /// <param name="key">The key associated with the validation error.</param>
    /// <param name="message">The validation error message.</param>
    /// <returns>A <see cref="ValidationFailedResult"/>.</returns>
    public static Result ValidationFailed(string key, string message)
        => new ValidationFailedResult(new[] { new ValidationError(key, message) });

    /// <summary>
    /// Creates a precondition failed result for the specified <see cref="PreconditionFailedReason"/>.
    /// </summary>
    /// <param name="reason">The reason for the precondition failure.</param>
    /// <param name="message">The optional message associated with the precondition failure.</param>
    /// <returns>A <see cref="PreconditionFailedResult"/>.</returns>
    public static Result PreconditionFailed(PreconditionFailedReason reason, string? message = null)
        => new PreconditionFailedResult(reason, message);
}

/// <summary>
/// Abstract representation of the result of an operation that has an associated value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public abstract class Result<T>(bool isSuccess) : Result(isSuccess)
{
    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A <see cref="SuccessResult{T}"/>.</returns>
    public static Result<T> Success(T value)
        => new SuccessResult<T>(value);

    /// <summary>
    /// Creates an unauthorized result.
    /// </summary>
    /// <param name="message">The unauthorized message.</param>
    /// <returns>An <see cref="UnauthorizedResult{T}"/>.</returns>
    public new static Result<T> Unauthorized(string message)
        => new UnauthorizedResult<T>(message);

    /// <summary>
    /// Creates a validation failed result for a collection of <see cref="ValidationError"/> objects.
    /// </summary>
    /// <param name="errors">The validation errors.</param>
    /// <returns>A <see cref="ValidationFailedResult{T}"/>.</returns>
    public new static Result<T> ValidationFailed(IReadOnlyCollection<ValidationError> errors)
        => new ValidationFailedResult<T>(errors);

    /// <summary>
    /// Creates a validation failed result for a single key with a collection of <see cref="ValidationError"/> objects.
    /// </summary>
    /// <param name="key">The key associated with the validation error.</param>
    /// <param name="errors">The validation errors.</param>
    /// <returns>A <see cref="ValidationFailedResult{T}"/>.</returns>
    public static Result<T> ValidationFailed(string key, IEnumerable<ValidationError> errors)
        => new ValidationFailedResult<T>(errors.Select(e => e with { Key = key }).ToList());

    /// <summary>
    /// Creates a validation failed result for a single key and message.
    /// </summary>
    /// <param name="key">The key associated with the validation error.</param>
    /// <param name="message">The validation error message.</param>
    /// <returns>A <see cref="ValidationFailedResult{T}"/>.</returns>
    public new static Result<T> ValidationFailed(string key, string message)
        => new ValidationFailedResult<T>([new ValidationError(key, message)]);

    /// <summary>
    /// Creates a precondition failed result.
    /// </summary>
    /// <param name="reason">The reason for the precondition failure.</param>
    /// <param name="message">The optional message associated with the precondition failure.</param>
    /// <returns>A <see cref="PreconditionFailedResult{T}"/>.</returns>
    public new static Result<T> PreconditionFailed(PreconditionFailedReason reason, string? message = null)
        => new PreconditionFailedResult<T>(reason, message);

    /// <summary>
    /// Maps the success result to another type.
    /// </summary>
    /// <typeparam name="TOther">The type to map to.</typeparam>
    /// <param name="successMapper">The function to map the success value.</param>
    /// <returns>A <see cref="Result{TOther}"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the result type is not known.</exception>
    public Result<TOther> Map<TOther>(Func<T, TOther> successMapper) =>
        this switch
        {
            SuccessResult<T> success => Result<TOther>.Success(successMapper(success.Value)),
            _ => MapFailure<TOther>()
        };

    /// <summary>
    /// Maps the failure result to another type.
    /// </summary>
    /// <typeparam name="TOther">The type to map to.</typeparam>
    /// <returns>A <see cref="Result{TOther}"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the result type is not known.</exception>
    public Result<TOther> MapFailure<TOther>() =>
        this switch
        {
            SuccessResult<T> => throw new InvalidOperationException("Cannot map failure on success result"),
            ValidationFailedResult<T> validationFailed => Result<TOther>.ValidationFailed(validationFailed.Errors),
            UnauthorizedResult<T> unauthorized => Result<TOther>.Unauthorized(unauthorized.Message),
            PreconditionFailedResult<T> preconditionFailed => Result<TOther>.PreconditionFailed(preconditionFailed.Reason),
            _ => throw new InvalidOperationException("Unknown result type")
        };

    /// <summary>
    /// Logs the failure result to the specified <see cref="ILogger"/>.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="logLevel">The log level.</param>
    /// <exception cref="InvalidOperationException">Thrown if the result is a success or an unknown type.</exception>
    public void LogFailure(ILogger logger, LogLevel logLevel = LogLevel.Warning)
    {
        switch (this)
        {
            case SuccessResult<T>:
                throw new InvalidOperationException("Cannot log failure for success result");
            case ValidationFailedResult<T> validationFailed:
                logger.Log(logLevel, "Validation failed: {Errors}", validationFailed.Errors);
                break;
            case UnauthorizedResult<T> unauthorized:
                logger.Log(logLevel, "Unauthorized: {Message}", unauthorized.Message);
                break;
            case PreconditionFailedResult<T> preconditionFailed:
                logger.Log(logLevel, "Precondition failed: {Reason}", preconditionFailed.Reason);
                break;
            default:
                throw new InvalidOperationException("Unknown result type");
        };
    }
}
