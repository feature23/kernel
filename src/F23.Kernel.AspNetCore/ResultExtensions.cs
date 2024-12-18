using System.Net;
using F23.Hateoas;
using F23.Kernel.Results;
using Microsoft.AspNetCore.Mvc;
using UnauthorizedResult = Microsoft.AspNetCore.Mvc.UnauthorizedResult;

namespace F23.Kernel.AspNetCore;

public static class ResultExtensions
{
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
}
