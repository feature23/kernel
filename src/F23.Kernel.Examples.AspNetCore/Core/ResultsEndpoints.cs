using F23.Kernel.AspNetCore;
using F23.Kernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace F23.Kernel.Examples.AspNetCore.Core;

/// <summary>
/// Minimal API endpoints demonstrating the different types of results available in F23.Kernel.
/// </summary>
public static class ResultsEndpoints
{
    /// <summary>
    /// Maps minimal API endpoints for demonstrating result types.
    /// </summary>
    /// <param name="app">The web application builder.</param>
    public static void MapResultsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/minimal-apis/results")
            .WithTags("Results - Minimal APIs");

        group.MapGet("/success-no-value", SuccessNoValue)
            .WithName("MinimalApiSuccessNoValue")
            .WithSummary("Demonstrates a successful result with no value")
            .WithDescription("Returns 204 No Content")
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/success-with-value", SuccessWithValue)
            .WithName("MinimalApiSuccessWithValue")
            .WithSummary("Demonstrates a successful result with a value")
            .WithDescription("Returns 200 OK with the value in the response body")
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/validation-failed", ValidationFailed)
            .WithName("MinimalApiValidationFailed")
            .WithSummary("Demonstrates a validation failed result")
            .WithDescription("Returns 400 Bad Request with validation errors")
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapGet("/unauthorized", Unauthorized)
            .WithName("MinimalApiUnauthorized")
            .WithSummary("Demonstrates an unauthorized result")
            .WithDescription("Returns 403 Forbidden")
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden);

        group.MapGet("/not-found", NotFound)
            .WithName("MinimalApiNotFound")
            .WithSummary("Demonstrates a precondition failed result (not found)")
            .WithDescription("Returns 404 Not Found")
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("/concurrency-mismatch", ConcurrencyMismatch)
            .WithName("MinimalApiConcurrencyMismatch")
            .WithSummary("Demonstrates a precondition failed result (concurrency mismatch)")
            .WithDescription("Returns 412 Precondition Failed")
            .Produces<ProblemDetails>(StatusCodes.Status412PreconditionFailed);

        group.MapGet("/conflict", Conflict)
            .WithName("MinimalApiConflict")
            .WithSummary("Demonstrates a precondition failed result (conflict)")
            .WithDescription("Returns 409 Conflict")
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);
    }

    private static IResult SuccessNoValue()
    {
        var result = Result.Success();
        return result.ToMinimalApiResult();
    }

    private static IResult SuccessWithValue()
    {
        var data = new { message = "Operation completed successfully", timestamp = DateTime.UtcNow };
        var result = Result<object>.Success(data);
        return result.ToMinimalApiResult();
    }

    private static IResult ValidationFailed()
    {
        var errors = new[]
        {
            new ValidationError("email", "Email address is invalid"),
            new ValidationError("password", "Password must be at least 8 characters"),
            new ValidationError("username", "Username is already taken"),
        };
        var result = Result.ValidationFailed(errors);
        return result.ToMinimalApiResult();
    }

    private static IResult Unauthorized()
    {
        var result = Result.Unauthorized("User does not have permission to access this resource");
        return result.ToMinimalApiResult();
    }

    private static IResult NotFound()
    {
        var result = Result.PreconditionFailed(
            PreconditionFailedReason.NotFound,
            "The requested resource was not found"
        );
        return result.ToMinimalApiResult();
    }

    private static IResult ConcurrencyMismatch()
    {
        var result = Result.PreconditionFailed(
            PreconditionFailedReason.ConcurrencyMismatch,
            "The resource has been modified by another process. Please refresh and try again."
        );
        return result.ToMinimalApiResult();
    }

    private static IResult Conflict()
    {
        var result = Result.PreconditionFailed(
            PreconditionFailedReason.Conflict,
            "The operation conflicts with the current state of the resource"
        );
        return result.ToMinimalApiResult();
    }
}
