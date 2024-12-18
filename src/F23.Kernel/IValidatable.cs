using F23.Kernel.Results;

namespace F23.Kernel;

public interface IValidatable
{
    ValidationResult Validate();
}
