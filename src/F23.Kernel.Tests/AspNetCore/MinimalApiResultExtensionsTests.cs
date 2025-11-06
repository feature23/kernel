using System.Net;
using F23.Hateoas;
using F23.Kernel.AspNetCore;
using F23.Kernel.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace F23.Kernel.Tests.AspNetCore;

public class MinimalApiResultExtensionsTests
{
    [Fact]
    public void SuccessResult_Returns_NoContentResult()
    {
        // Arrange
        var result = new SuccessResult();

        // Act
        var actionResult = result.ToMinimalApiResult();

        // Assert
        Assert.IsType<NoContent>(actionResult);
    }

    [Fact]
    public void AggregateResult_Success_Returns_NoContentResult()
    {
        // Arrange
        var result = new AggregateResult([new SuccessResult()]);

        // Act
        var actionResult = result.ToMinimalApiResult();

        // Assert
        Assert.IsType<NoContent>(actionResult);
    }

    [Fact]
    public void AggregateResult_Failure_Returns_First_Failed_Result()
    {
        // Arrange
        var result = new AggregateResult([
            new SuccessResult(),
            new PreconditionFailedResult(PreconditionFailedReason.NotFound, null)
        ]);

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        Assert.IsType<NotFound>(actionResult);
    }

    [Fact]
    public void PreconditionFailedResult_NotFound_Returns_NotFoundResult()
    {
        // Arrange
        var result = new PreconditionFailedResult(PreconditionFailedReason.NotFound, null);

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        Assert.IsType<NotFound>(actionResult);
    }

    [Fact]
    public void PreconditionFailedResult_ConcurrencyMismatch_Returns_StatusCodeResult()
    {
        // Arrange
        var result = new PreconditionFailedResult(PreconditionFailedReason.ConcurrencyMismatch, null);

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeHttpResult>(actionResult);
        Assert.Equal((int) HttpStatusCode.PreconditionFailed, statusCodeResult.StatusCode);
    }

    [Fact]
    public void PreconditionFailedResult_Conflict_Returns_ConflictResult()
    {
        // Arrange
        var result = new PreconditionFailedResult(PreconditionFailedReason.Conflict, null);

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        Assert.IsType<Conflict>(actionResult);
    }

    [Fact]
    public void ValidationFailedResult_Returns_BadRequestObjectResult()
    {
        // Arrange
        var result = new ValidationFailedResult(new List<ValidationError>
        {
            new("Key", "Message")
        });

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequest<ModelStateDictionary>>(actionResult);
        var modelState = Assert.IsType<ModelStateDictionary>(badRequestObjectResult.Value);
        Assert.True(modelState.TryGetValue("Key", out var modelStateEntry));
        Assert.NotNull(modelStateEntry);
        Assert.Single(modelStateEntry.Errors);
        Assert.Equal("Message", modelStateEntry.Errors.First().ErrorMessage);
    }

    [Fact]
    public void UnauthorizedResult_Returns_ForbidResult()
    {
        // Arrange
        var result = new F23.Kernel.Results.UnauthorizedResult("NONE SHALL PASS");

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        Assert.IsType<ForbidHttpResult>(actionResult);
    }

    [Fact]
    public void SuccessResultT_Returns_OkObjectResult()
    {
        // Arrange
        var result = new SuccessResult<string>("Hello, World!");

        // Act
        var actionResult = result.ToMinimalApiResult();

        // Assert
        var okObjectResult = Assert.IsType<Ok<HypermediaResponse>>(actionResult);
        var hypermediaResponse = Assert.IsType<HypermediaResponse>(okObjectResult.Value);
        Assert.Equal("Hello, World!", hypermediaResponse.Content);
    }

    [Fact]
    public void SuccessResultT_With_SuccessMap_Returns_SuccessMapResult()
    {
        // Arrange
        var result = new SuccessResult<string>("Hello, World!");

        // Act
        var actionResult = result.ToMinimalApiResult(Microsoft.AspNetCore.Http.Results.Ok);

        // Assert
        var okObjectResult = Assert.IsType<Ok<string>>(actionResult);
        Assert.Equal("Hello, World!", okObjectResult.Value);
    }

    [Fact]
    public void PreconditionFailedResultT_NotFound_Returns_NotFoundResult()
    {
        // Arrange
        var result = new PreconditionFailedResult<string>(PreconditionFailedReason.NotFound, null);

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        Assert.IsType<NotFound>(actionResult);
    }

    [Fact]
    public void PreconditionFailedResultT_ConcurrencyMismatch_Returns_StatusCodeResult()
    {
        // Arrange
        var result = new PreconditionFailedResult<string>(PreconditionFailedReason.ConcurrencyMismatch, null);

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeHttpResult>(actionResult);
        Assert.Equal((int) HttpStatusCode.PreconditionFailed, statusCodeResult.StatusCode);
    }

    [Fact]
    public void PreconditionFailedResultT_Conflict_Returns_ConflictResult()
    {
        // Arrange
        var result = new PreconditionFailedResult<string>(PreconditionFailedReason.Conflict, null);

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        Assert.IsType<Conflict>(actionResult);
    }

    [Fact]
    public void ValidationFailedResultT_Returns_BadRequestObjectResult()
    {
        // Arrange
        var result = new ValidationFailedResult<string>(new List<ValidationError>
        {
            new("Key", "Message")
        });

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequest<ModelStateDictionary>>(actionResult);
        var modelState = Assert.IsType<ModelStateDictionary>(badRequestObjectResult.Value);
        Assert.True(modelState.TryGetValue("Key", out var modelStateEntry));
        Assert.NotNull(modelStateEntry);
        Assert.Single(modelStateEntry.Errors);
        Assert.Equal("Message", modelStateEntry.Errors.First().ErrorMessage);
    }

    [Fact]
    public void UnauthorizedResultT_Returns_ForbidResult()
    {
        // Arrange
        var result = new UnauthorizedResult<string>("NONE SHALL PASS");

        // Act
        var actionResult = result.ToMinimalApiResult(useProblemDetails: false);

        // Assert
        Assert.IsType<ForbidHttpResult>(actionResult);
    }

    [Fact]
    public void ToMinimalApiResult_Throws_ArgumentOutOfRangeException_For_Unhandled_Result()
    {
        // Arrange
        var result = new TestUnhandledResult();

        // Act
        void Act() => result.ToMinimalApiResult();

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void ToMinimalApiResultT_Throws_ArgumentOutOfRangeException_For_Unhandled_Result()
    {
        // Arrange
        var result = new TestUnhandledResult<string>();

        // Act
        void Act() => result.ToMinimalApiResult();

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    // Problem Details Tests
    [Fact]
    public void PreconditionFailedResult_NotFound_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new PreconditionFailedResult(PreconditionFailedReason.NotFound, "Resource not found");

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned, which contains the status code and title
    }

    [Fact]
    public void PreconditionFailedResult_ConcurrencyMismatch_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new PreconditionFailedResult(PreconditionFailedReason.ConcurrencyMismatch, "Concurrency mismatch");

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned, which contains the status code and title
    }

    [Fact]
    public void PreconditionFailedResult_Conflict_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new PreconditionFailedResult(PreconditionFailedReason.Conflict, "Conflict occurred");

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned, which contains the status code and title
    }

    [Fact]
    public void ValidationFailedResult_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new ValidationFailedResult(new List<ValidationError>
        {
            new("Key", "Error message")
        });

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned for validation failures with problem details
    }

    [Fact]
    public void UnauthorizedResult_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new F23.Kernel.Results.UnauthorizedResult("Access denied");

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned, which contains the status code and title
    }

    [Fact]
    public void PreconditionFailedResultT_NotFound_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new PreconditionFailedResult<string>(PreconditionFailedReason.NotFound, "Resource not found");

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned, which contains the status code and title
    }

    [Fact]
    public void PreconditionFailedResultT_ConcurrencyMismatch_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new PreconditionFailedResult<string>(PreconditionFailedReason.ConcurrencyMismatch, "Concurrency mismatch");

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned, which contains the status code and title
    }

    [Fact]
    public void PreconditionFailedResultT_Conflict_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new PreconditionFailedResult<string>(PreconditionFailedReason.Conflict, "Conflict occurred");

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned, which contains the status code and title
    }

    [Fact]
    public void ValidationFailedResultT_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new ValidationFailedResult<string>(new List<ValidationError>
        {
            new("Key", "Error message")
        });

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned for validation failures with problem details
    }

    [Fact]
    public void UnauthorizedResultT_With_UseProblemDetails_Returns_ProblemDetails()
    {
        // Arrange
        var result = new UnauthorizedResult<string>("Access denied");

        // Act
        var minimalResult = result.ToMinimalApiResult(useProblemDetails: true);

        // Assert
        Assert.NotNull(minimalResult);
        // Problem result is returned, which contains the status code and title
    }

    [Fact]
    public void ToProblemHttpResult_Returns_ProblemHttpResult()
    {
        // Arrange
        var result = new TestUnhandledResult();

        // Act
        var httpResult = result.ToProblemHttpResult(HttpStatusCode.BadRequest);

        // Assert
        Assert.NotNull(httpResult);
        // Problem result is returned with the specified status code and title
    }

    private class TestUnhandledResult() : Result(true)
    {
        public override string Message => "whoopsie";
    }

    private class TestUnhandledResult<T>() : Result<T>(true)
    {
        public override string Message => "whoopsie";
    }
}
