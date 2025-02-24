using System.Net;
using F23.Hateoas;
using F23.Kernel.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UnauthorizedResult = Microsoft.AspNetCore.Mvc.UnauthorizedResult;

namespace F23.Kernel.AspNetCore;

/// <summary>
/// Provides extension methods for converting <see cref="Result"/> objects to <see cref="IActionResult"/> objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a <see cref="Result"/> into an appropriate <see cref="IActionResult"/>
    /// that represents the result to be sent in an HTTP response.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> to be converted.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the HTTP response:
    /// <list type="bullet">
    /// <item>A <see cref="NoContentResult"/> for successful results.</item>
    /// <item>A <see cref="NotFoundResult"/> for a <see cref="PreconditionFailedResult"/> indicating <see cref="PreconditionFailedReason.NotFound"/>.</item>
    /// <item>A <see cref="StatusCodeResult"/> with HTTP status code 412 for a <see cref="PreconditionFailedResult"/> indicating <see cref="PreconditionFailedReason.ConcurrencyMismatch"/>.</item>
    /// <item>A <see cref="ConflictResult"/> for a <see cref="PreconditionFailedResult"/> indicating <see cref="PreconditionFailedReason.Conflict"/>.</item>
    /// <item>A <see cref="BadRequestObjectResult"/> with model state populated for a <see cref="ValidationFailedResult"/>.</item>
    /// <item>An <see cref="F23.Kernel.Results.UnauthorizedResult"/> in case of an <see cref="UnauthorizedResult"/>.</item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <paramref name="result"/> does not match any known result types.
    /// </exception>
    public static IActionResult ToActionResult(this Result result)
        => result switch
        {
            SuccessResult => new NoContentResult(),
            AggregateResult { IsSuccess: true } => new NoContentResult(),
            AggregateResult { IsSuccess: false, Results.Count: > 0 } aggregateResult =>  aggregateResult.Results.First(i => !i.IsSuccess).ToActionResult(),
            PreconditionFailedResult { Reason: PreconditionFailedReason.NotFound } => new NotFoundResult(),
            PreconditionFailedResult { Reason: PreconditionFailedReason.ConcurrencyMismatch } => new StatusCodeResult((int) HttpStatusCode.PreconditionFailed),
            PreconditionFailedResult { Reason: PreconditionFailedReason.Conflict } => new ConflictResult(),
            ValidationFailedResult validationFailed => new BadRequestObjectResult(validationFailed.Errors.ToModelState()),
            F23.Kernel.Results.UnauthorizedResult => new UnauthorizedResult(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };

    /// <summary>
    /// Converts a <see cref="Result"/> into an appropriate <see cref="IResult"/> to be used in a minimal API context.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> to be converted.</param>
    /// <returns>
    /// An <see cref="IResult"/> that represents the appropriate response:
    /// - An <see cref="NoContent"/> for a <see cref="SuccessResult"/>.
    /// - A <see cref="NotFound"/> for a <see cref="PreconditionFailedResult"/> with a reason of <see cref="PreconditionFailedReason.NotFound"/>.
    /// - A <see cref="StatusCodeHttpResult"/> with status code 412 for a <see cref="PreconditionFailedResult"/> with a reason of <see cref="PreconditionFailedReason.ConcurrencyMismatch"/>.
    /// - A <see cref="Conflict"/> for a <see cref="PreconditionFailedResult"/> with a reason of <see cref="PreconditionFailedReason.Conflict"/>.
    /// - A <see cref="BadRequest"/> with model state populated for a <see cref="ValidationFailedResult"/>.
    /// - An <see cref="UnauthorizedHttpResult"/> for an <see cref="UnauthorizedResult"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <paramref name="result"/> does not match any known result types.
    /// </exception>
    public static IResult ToMinimalApiResult(this Result result)
        => result switch
        {
            SuccessResult => Microsoft.AspNetCore.Http.Results.NoContent(),
            AggregateResult { IsSuccess: true } => Microsoft.AspNetCore.Http.Results.NoContent(),
            AggregateResult { IsSuccess: false, Results.Count: > 0 } aggregateResult =>
                aggregateResult.Results.First(i => !i.IsSuccess).ToMinimalApiResult(),
            PreconditionFailedResult { Reason: PreconditionFailedReason.NotFound } =>
                Microsoft.AspNetCore.Http.Results.NotFound(),
            PreconditionFailedResult { Reason: PreconditionFailedReason.ConcurrencyMismatch } =>
                Microsoft.AspNetCore.Http.Results.StatusCode((int) HttpStatusCode.PreconditionFailed),
            PreconditionFailedResult { Reason: PreconditionFailedReason.Conflict } =>
                Microsoft.AspNetCore.Http.Results.Conflict(),
            ValidationFailedResult validationFailed =>
                Microsoft.AspNetCore.Http.Results.BadRequest(validationFailed.Errors.ToModelState()),
            F23.Kernel.Results.UnauthorizedResult => Microsoft.AspNetCore.Http.Results.Unauthorized(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };

    /// <summary>
    /// Converts a <see cref="Result"/> into an appropriate <see cref="IActionResult"/>
    /// that represents the result to be sent in an HTTP response.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result, if successful.</typeparam>
    /// <param name="result">The result instance to convert.</param>
    /// <param name="successMap">
    /// An optional function to map a successful result to a custom <see cref="IActionResult"/>.
    /// If not provided, a default mapping is applied.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the HTTP response:
    /// <list type="bullet">
    /// <item>The result of <paramref name="successMap"/>, if specified, for a <see cref="SuccessResult"/>.</item>
    /// <item>An <see cref="OkObjectResult"/> for a <see cref="SuccessResult"/>, when <paramref name="successMap"/> is not specified.</item>
    /// <item>A <see cref="NotFoundResult"/> for a <see cref="PreconditionFailedResult"/> indicating <see cref="PreconditionFailedReason.NotFound"/>.</item>
    /// <item>A <see cref="StatusCodeResult"/> with HTTP status code 412 for a <see cref="PreconditionFailedResult"/> indicating <see cref="PreconditionFailedReason.ConcurrencyMismatch"/>.</item>
    /// <item>A <see cref="ConflictResult"/> for a <see cref="PreconditionFailedResult"/> indicating <see cref="PreconditionFailedReason.Conflict"/>.</item>
    /// <item>A <see cref="BadRequestObjectResult"/> with model state populated for a <see cref="ValidationFailedResult"/>.</item>
    /// <item>An <see cref="F23.Kernel.Results.UnauthorizedResult"/> in case of an <see cref="UnauthorizedResult"/>.</item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <paramref name="result"/> does not match any known result types.
    /// </exception>
    public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult>? successMap = null)
        => result switch
        {
            SuccessResult<T> success when successMap != null => successMap(success.Value),
            SuccessResult<T> success => new OkObjectResult(new HypermediaResponse(success.Value)),
            PreconditionFailedResult<T> { Reason: PreconditionFailedReason.NotFound } => new NotFoundResult(),
            PreconditionFailedResult<T> { Reason: PreconditionFailedReason.ConcurrencyMismatch } => new StatusCodeResult((int) HttpStatusCode.PreconditionFailed),
            PreconditionFailedResult<T> { Reason: PreconditionFailedReason.Conflict } => new ConflictResult(),
            ValidationFailedResult<T> validationFailed => new BadRequestObjectResult(validationFailed.Errors.ToModelState()),
            UnauthorizedResult<T> => new UnauthorizedResult(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };

    /// <summary>
    /// Converts a <see cref="Result{T}"/> into an appropriate <see cref="IResult"/> to be used in a minimal API context.
    /// </summary>
    /// <param name="result">The <see cref="Result{T}"/> instance to be converted.</param>
    /// <param name="successMap">
    /// An optional function to map the value of a successful result into a user-defined <see cref="IResult"/>.
    /// If not provided, successful results will default to an HTTP 200 response with the value serialized as the body.
    /// </param>
    /// <typeparam name="T">The type of the result's value.</typeparam>
    /// <returns>
    /// An <see cref="IResult"/> that represents the appropriate response:
    /// - An HTTP 200 (OK) response for a <see cref="SuccessResult{T}"/> if <paramref name="successMap"/> is not provided.
    /// - The result of <paramref name="successMap"/> if provided and the result is a <see cref="SuccessResult{T}"/>.
    /// - An HTTP 404 (NotFound) response for a <see cref="PreconditionFailedResult{T}"/> with a reason of <see cref="PreconditionFailedReason.NotFound"/>.
    /// - An HTTP 412 (PreconditionFailed) response for a <see cref="PreconditionFailedResult{T}"/> with a reason of <see cref="PreconditionFailedReason.ConcurrencyMismatch"/>.
    /// - An HTTP 409 (Conflict) response for a <see cref="PreconditionFailedResult{T}"/> with a reason of <see cref="PreconditionFailedReason.Conflict"/>.
    /// - An HTTP 400 (BadRequest) response with model state populated for a <see cref="ValidationFailedResult{T}"/>.
    /// - An HTTP 401 (Unauthorized) response for an <see cref="UnauthorizedResult{T}"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <paramref name="result"/> does not match any known result types.
    /// </exception>
    public static IResult ToMinimalApiResult<T>(this Result<T> result, Func<T, IResult>? successMap = null)
        => result switch
        {
            SuccessResult<T> success when successMap != null => successMap(success.Value),
            SuccessResult<T> success => Microsoft.AspNetCore.Http.Results.Ok(new HypermediaResponse(success.Value)),
            PreconditionFailedResult<T> { Reason: PreconditionFailedReason.NotFound } =>
                Microsoft.AspNetCore.Http.Results.NotFound(),
            PreconditionFailedResult<T> { Reason: PreconditionFailedReason.ConcurrencyMismatch } =>
                Microsoft.AspNetCore.Http.Results.StatusCode((int)HttpStatusCode.PreconditionFailed),
            PreconditionFailedResult<T> { Reason: PreconditionFailedReason.Conflict } =>
                Microsoft.AspNetCore.Http.Results.Conflict(),
            ValidationFailedResult<T> validationFailed =>
                Microsoft.AspNetCore.Http.Results.BadRequest(validationFailed.Errors.ToModelState()),
            UnauthorizedResult<T> => Microsoft.AspNetCore.Http.Results.Unauthorized(),
            _ => throw new ArgumentOutOfRangeException(nameof(result)),
        };
}
