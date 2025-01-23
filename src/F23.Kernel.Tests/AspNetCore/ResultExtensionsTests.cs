using System.Net;
using F23.Hateoas;
using F23.Kernel.AspNetCore;
using F23.Kernel.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace F23.Kernel.Tests.AspNetCore;

public class ResultExtensionsTests
{
    [Fact]
    public void SuccessResult_Returns_NoContentResult()
    {
        // Arrange
        var result = new SuccessResult();

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public void AggregateResult_Success_Returns_NoContentResult()
    {
        // Arrange
        var result = new AggregateResult([new SuccessResult()]);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        Assert.IsType<NoContentResult>(actionResult);
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
        var actionResult = result.ToActionResult();

        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }

    [Fact]
    public void PreconditionFailedResult_NotFound_Returns_NotFoundResult()
    {
        // Arrange
        var result = new PreconditionFailedResult(PreconditionFailedReason.NotFound, null);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }

    [Fact]
    public void PreconditionFailedResult_ConcurrencyMismatch_Returns_StatusCodeResult()
    {
        // Arrange
        var result = new PreconditionFailedResult(PreconditionFailedReason.ConcurrencyMismatch, null);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(actionResult);
        Assert.Equal((int) HttpStatusCode.PreconditionFailed, statusCodeResult.StatusCode);
    }

    [Fact]
    public void PreconditionFailedResult_Conflict_Returns_ConflictResult()
    {
        // Arrange
        var result = new PreconditionFailedResult(PreconditionFailedReason.Conflict, null);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        Assert.IsType<ConflictResult>(actionResult);
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
        var actionResult = result.ToActionResult();

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult);
        var modelState = Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        Assert.True(modelState.TryGetValue("Key", out var value));
        var errors = Assert.IsType<string[]>(value);
        var message = Assert.Single(errors);
        Assert.Equal("Message", message);
    }

    [Fact]
    public void UnauthorizedResult_Returns_UnauthorizedResult()
    {
        // Arrange
        var result = new F23.Kernel.Results.UnauthorizedResult("NONE SHALL PASS");

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.UnauthorizedResult>(actionResult);
    }

    [Fact]
    public void SuccessResultT_Returns_OkObjectResult()
    {
        // Arrange
        var result = new SuccessResult<string>("Hello, World!");

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
        var hypermediaResponse = Assert.IsType<HypermediaResponse>(okObjectResult.Value);
        Assert.Equal("Hello, World!", hypermediaResponse.Content);
    }

    [Fact]
    public void SuccessResultT_With_SuccessMap_Returns_SuccessMapResult()
    {
        // Arrange
        var result = new SuccessResult<string>("Hello, World!");

        // Act
        var actionResult = result.ToActionResult(value => new OkObjectResult(value));

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal("Hello, World!", okObjectResult.Value);
    }

    [Fact]
    public void PreconditionFailedResultT_NotFound_Returns_NotFoundResult()
    {
        // Arrange
        var result = new PreconditionFailedResult<string>(PreconditionFailedReason.NotFound, null);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }

    [Fact]
    public void PreconditionFailedResultT_ConcurrencyMismatch_Returns_StatusCodeResult()
    {
        // Arrange
        var result = new PreconditionFailedResult<string>(PreconditionFailedReason.ConcurrencyMismatch, null);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(actionResult);
        Assert.Equal((int) HttpStatusCode.PreconditionFailed, statusCodeResult.StatusCode);
    }

    [Fact]
    public void PreconditionFailedResultT_Conflict_Returns_ConflictResult()
    {
        // Arrange
        var result = new PreconditionFailedResult<string>(PreconditionFailedReason.Conflict, null);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        Assert.IsType<ConflictResult>(actionResult);
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
        var actionResult = result.ToActionResult();

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult);
        var modelState = Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        Assert.True(modelState.TryGetValue("Key", out var value));
        var errors = Assert.IsType<string[]>(value);
        var message = Assert.Single(errors);
        Assert.Equal("Message", message);
    }

    [Fact]
    public void UnauthorizedResultT_Returns_UnauthorizedResult()
    {
        // Arrange
        var result = new UnauthorizedResult<string>("NONE SHALL PASS");

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        Assert.IsType<Microsoft.AspNetCore.Mvc.UnauthorizedResult>(actionResult);
    }

    [Fact]
    public void ToActionResult_Throws_ArgumentOutOfRangeException_For_Unhandled_Result()
    {
        // Arrange
        var result = new TestUnhandledResult();

        // Act
        void Act() => result.ToActionResult();

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void ToActionResultT_Throws_ArgumentOutOfRangeException_For_Unhandled_Result()
    {
        // Arrange
        var result = new TestUnhandledResult<string>();

        // Act
        void Act() => result.ToActionResult();

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
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
