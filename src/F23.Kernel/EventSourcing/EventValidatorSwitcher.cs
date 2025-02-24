using F23.Kernel.Results;

namespace F23.Kernel.EventSourcing;

/// <summary>
/// The EventValidatorSwitcher class handles the validation of events
/// within an event stream using dependency injection to find and invoke
/// the appropriate validators based on the event type.
/// </summary>
/// <param name="serviceProvider">An <see cref="IServiceProvider"/> for resolving validators.</param>
/// <remarks>
/// This class resolves event-specific validators via dependency injection,
/// validates the given event, and aggregates validation errors, if any,
/// from multiple applicable validators.
/// </remarks>
public class EventValidatorSwitcher(IServiceProvider serviceProvider)
{
    /// <summary>
    /// Validates the provided event within the context of an event stream, utilizing all applicable validators.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root associated with the event stream.</typeparam>
    /// <param name="eventStream">The event stream containing the aggregate root and its associated events.</param>
    /// <param name="e">The event to validate.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a
    /// <see cref="ValidationResult"/> indicating whether the validation passed or failed.
    /// If any errors occur during validation, they will be returned as part of a failed result.
    /// </returns>
    public async Task<ValidationResult> Validate<T>(EventStream<T> eventStream,
        IEvent e,
        CancellationToken cancellationToken = default)
        where T : IAggregateRoot
    {
        var validatorType = MakeValidatorTypeFromEvent<T>(e);
        var validators = serviceProvider.GetServices(validatorType);
        var errors = new List<ValidationError>();

        foreach (object? validator in validators)
        {
            if (validator == null)
            {
                continue;
            }

            var result = await ValidateEvent(eventStream, e, validatorType, validator, cancellationToken);

            if (result is ValidationFailedResult failedResult)
            {
                errors.AddRange(failedResult.Errors);
            }
        }

        return errors.Count > 0
            ? ValidationResult.Failed(errors)
            : ValidationResult.Passed();
    }

    private static async Task<ValidationResult> ValidateEvent<T>(EventStream<T> eventStream,
        IEvent e,
        Type validatorType,
        object? validator,
        CancellationToken cancellationToken) where T : IAggregateRoot
    {
        var method = validatorType.GetMethod(nameof(IEventValidator<T, IEvent>.Validate))!;
        return await (Task<ValidationResult>)method.Invoke(validator, [eventStream, e, cancellationToken])!;
    }

    private static Type MakeValidatorTypeFromEvent<T>(IEvent e)
        where T : IAggregateRoot
        => typeof(IEventValidator<,>).MakeGenericType(typeof(T), e.GetType());
}
