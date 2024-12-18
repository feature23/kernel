using F23.Kernel.Results;

namespace F23.Kernel.EventSourcing;

public class EventValidatorSwitcher(IServiceProvider serviceProvider)
{
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
