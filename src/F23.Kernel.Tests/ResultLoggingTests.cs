using F23.Kernel.Results;
using F23.Kernel.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace F23.Kernel.Tests;

public class ResultLoggingTests
{
    [Fact]
    public void LogFailure_SuccessResult_Throws()
    {
        // Arrange
        var result = Result<TestResultContent>.Success(new TestResultContent());

        // Act
        void Act() => result.LogFailure(NullLogger.Instance);

        // Assert
        Assert.Throws<InvalidOperationException>(Act);
    }

    [Fact]
    public void LogFailure_ValidationFailedResult_Logs()
    {
        // Arrange
        var logger = new TestLogger();
        var result = Result<TestResultContent>.ValidationFailed("key", "message");

        // Act
        result.LogFailure(logger);

        // Assert
        var message = Assert.Single(logger.Messages);
        Assert.Equal("Validation failed: ValidationError { Key = key, Message = message }", message);
    }

    [Fact]
    public void LogFailure_UnauthorizedResult_Logs()
    {
        // Arrange
        var logger = new TestLogger();
        var result = Result<TestResultContent>.Unauthorized("YOU SHALL NOT PASS");

        // Act
        result.LogFailure(logger);

        // Assert
        var message = Assert.Single(logger.Messages);
        Assert.Equal("Unauthorized: YOU SHALL NOT PASS", message);
    }

    [Fact]
    public void LogFailure_PreconditionFailedResult_Logs()
    {
        // Arrange
        var logger = new TestLogger();
        var result = Result<TestResultContent>.PreconditionFailed(PreconditionFailedReason.NotFound, "Not found");

        // Act
        result.LogFailure(logger);

        // Assert
        var message = Assert.Single(logger.Messages);
        Assert.Equal("Precondition failed: NotFound", message);
    }

    [Fact]
    public void LogFailure_UnknownResultType_Throws()
    {
        // Arrange
        var logger = new TestLogger();
        var result = new UnknownResult();

        // Act
        void Act() => result.LogFailure(logger);

        // Assert
        Assert.Throws<InvalidOperationException>(Act);
    }

    private class TestLogger : ILogger
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)  where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Messages.Add(formatter(state, exception));
        }
    }
}
