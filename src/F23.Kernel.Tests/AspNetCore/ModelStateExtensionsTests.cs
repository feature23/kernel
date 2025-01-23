using F23.Kernel.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace F23.Kernel.Tests.AspNetCore;

public class ModelStateExtensionsTests
{
    [Fact]
    public void AddModelErrors_AddsAllValidationErrors()
    {
        // Arrange
        var modelState = new ModelStateDictionary();
        var errors = new List<ValidationError>
        {
            new("key1", "error1"),
            new("key2", "error2")
        };

        // Act
        modelState.AddModelErrors(errors);

        // Assert
        Assert.Equal(2, modelState.ErrorCount);
        Assert.Equal("error1", modelState["key1"].Errors.First().ErrorMessage);
        Assert.Equal("error2", modelState["key2"].Errors.First().ErrorMessage);
    }

    [Fact]
    public void AddModelErrors_WithKey_AddsAllValidationErrorsWithKey()
    {
        // Arrange
        var modelState = new ModelStateDictionary();
        var errors = new List<ValidationError>
        {
            new("key1", "error1"),
            new("key2", "error2")
        };

        // Act
        modelState.AddModelErrors("key", errors);

        // Assert
        Assert.Equal(2, modelState.ErrorCount);
        var entry = modelState["key"];
        Assert.NotNull(entry);
        Assert.Equal(2, entry.Errors.Count);
        Assert.Equal("error1", entry.Errors.First().ErrorMessage);
        Assert.Equal("error2", entry.Errors.Last().ErrorMessage);
    }

    [Fact]
    public void ToModelState_ReturnsModelStateWithAllValidationErrors()
    {
        // Arrange
        var errors = new List<ValidationError>
        {
            new("key1", "error1"),
            new("key2", "error2")
        };

        // Act
        var modelState = errors.ToModelState();

        // Assert
        Assert.Equal(2, modelState.ErrorCount);
        Assert.Equal("error1", modelState["key1"].Errors.First().ErrorMessage);
        Assert.Equal("error2", modelState["key2"].Errors.First().ErrorMessage);
    }
}
