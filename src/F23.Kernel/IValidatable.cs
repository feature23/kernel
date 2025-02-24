using F23.Kernel.Results;

namespace F23.Kernel;

/// <summary>
/// Represents an entity that can validate itself.
/// </summary>
public interface IValidatable
{
    /// <summary>
    /// Validates the entity.
    /// </summary>
    /// <returns>A <see cref="ValidationResult"/> that contains the validation results.</returns>
    ValidationResult Validate();
}
