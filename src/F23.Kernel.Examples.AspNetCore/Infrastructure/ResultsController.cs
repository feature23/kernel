using F23.Kernel;
using F23.Kernel.AspNetCore;
using F23.Kernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace F23.Kernel.Examples.AspNetCore.Infrastructure;

/// <summary>
/// MVC controller demonstrating the different types of results available in F23.Kernel.
/// </summary>
[ApiController]
[Route("mvc/results")]
[Tags("Results - MVC")]
public class ResultsController : ControllerBase
{
    /// <summary>
    /// Demonstrates a successful result with no value.
    /// </summary>
    /// <returns>204 No Content</returns>
    [HttpGet("success-no-value")]
    public IActionResult SuccessNoValue()
    {
        var result = Result.Success();
        return result.ToActionResult();
    }

    /// <summary>
    /// Demonstrates a successful result with a value.
    /// </summary>
    /// <returns>200 OK with the value in the response body</returns>
    [HttpGet("success-with-value")]
    public IActionResult SuccessWithValue()
    {
        var data = new { message = "Operation completed successfully", timestamp = DateTime.UtcNow };
        var result = Result<object>.Success(data);
        return result.ToActionResult();
    }

    /// <summary>
    /// Demonstrates a validation failed result.
    /// </summary>
    /// <returns>400 Bad Request with validation errors</returns>
    [HttpGet("validation-failed")]
    public IActionResult ValidationFailed()
    {
        var errors = new[]
        {
            new ValidationError("email", "Email address is invalid"),
            new ValidationError("password", "Password must be at least 8 characters"),
            new ValidationError("username", "Username is already taken"),
        };
        var result = Result.ValidationFailed(errors);
        return result.ToActionResult();
    }

    /// <summary>
    /// Demonstrates an unauthorized result.
    /// </summary>
    /// <returns>403 Forbidden</returns>
    [HttpGet("unauthorized")]
    public IActionResult UnauthorizedDemo()
    {
        var result = Result.Unauthorized("User does not have permission to access this resource");
        return result.ToActionResult();
    }

    /// <summary>
    /// Demonstrates a precondition failed result (not found).
    /// </summary>
    /// <returns>404 Not Found</returns>
    [HttpGet("not-found")]
    public IActionResult NotFoundDemo()
    {
        var result = Result.PreconditionFailed(
            PreconditionFailedReason.NotFound,
            "The requested resource was not found"
        );
        return result.ToActionResult();
    }

    /// <summary>
    /// Demonstrates a precondition failed result (concurrency mismatch).
    /// </summary>
    /// <returns>412 Precondition Failed</returns>
    [HttpGet("concurrency-mismatch")]
    public IActionResult ConcurrencyMismatch()
    {
        var result = Result.PreconditionFailed(
            PreconditionFailedReason.ConcurrencyMismatch,
            "The resource has been modified by another process. Please refresh and try again."
        );
        return result.ToActionResult();
    }

    /// <summary>
    /// Demonstrates a precondition failed result (conflict).
    /// </summary>
    /// <returns>409 Conflict</returns>
    [HttpGet("conflict")]
    public IActionResult ConflictDemo()
    {
        var result = Result.PreconditionFailed(
            PreconditionFailedReason.Conflict,
            "The operation conflicts with the current state of the resource"
        );
        return result.ToActionResult();
    }
}
