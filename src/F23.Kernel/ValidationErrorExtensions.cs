namespace F23.Kernel;

/// <summary>
/// Extension methods related to <see cref="ValidationError"/>.
/// </summary>
public static class ValidationErrorExtensions
{
    /// <summary>
    /// Creates a dictionary of error messages from the given <paramref name="validationErrors"/>.
    /// </summary>
    /// <param name="validationErrors">The validation errors.</param>
    /// <returns>>A dictionary where the keys are the field names and the values are arrays of error messages.</returns>
    /// <remarks>
    /// This is adapted from MIT-licensed code from .NET, in Microsoft.AspNetCore.Mvc.ValidationProblemDetails.
    /// <para />
    /// License text in file:
    /// Licensed to the .NET Foundation under one or more agreements.
    /// The .NET Foundation licenses this file to you under the MIT license.
    /// </remarks>
    public static IDictionary<string, string[]> CreateErrorDictionary(this IReadOnlyCollection<ValidationError> validationErrors)
    {
        ArgumentNullException.ThrowIfNull(validationErrors);

        var errorDictionary = new Dictionary<string, string[]>(StringComparer.Ordinal);

        foreach (var (key, message) in validationErrors)
        {
            errorDictionary.Add(key, [message]);
        }

        return errorDictionary;
    }
}
