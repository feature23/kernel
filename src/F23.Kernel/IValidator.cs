using F23.Kernel.Results;

namespace F23.Kernel;

public interface IValidator<in T>
{
    Task<ValidationResult> Validate(T value, CancellationToken cancellationToken = default);
}