namespace F23.Kernel;

/// <summary>
/// Represents a validation error with a key and a message.
/// </summary>
/// <param name="Key">The key associated with the validation error.</param>
/// <param name="Message">The validation error message.</param>
public record ValidationError(string Key, string Message);
