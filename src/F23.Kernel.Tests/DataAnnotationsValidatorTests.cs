using System.ComponentModel.DataAnnotations;
using F23.Kernel.Results;

namespace F23.Kernel.Tests;

public class DataAnnotationsValidatorTests
{
    [Fact]
    public async Task Validate_WithValidObject_ReturnsPassedValidationResult()
    {
        // Arrange
        var validator = new DataAnnotationsValidator<TestValidatableObject>();
        var validObject = new TestValidatableObject { RequiredProperty = "Valid" };

        // Act
        var result = await validator.Validate(validObject);

        // Assert
        Assert.IsType<ValidationPassedResult>(result);
    }

    [Fact]
    public async Task Validate_WithInvalidObject_ReturnsFailedValidationResult()
    {
        // Arrange
        var validator = new DataAnnotationsValidator<TestValidatableObject>();
        var invalidObject = new TestValidatableObject();

        // Act
        var result = await validator.Validate(invalidObject);

        // Assert
        var failedResult = Assert.IsType<ValidationFailedResult>(result);
        var error = Assert.Single(failedResult.Errors);
        Assert.Equal(nameof(TestValidatableObject.RequiredProperty), error.Key);
        Assert.Equal($"The {nameof(TestValidatableObject.RequiredProperty)} field is required.", error.Message);
    }

    private class TestValidatableObject
    {
        [Required]
        public string? RequiredProperty { get; set; }
    }
}
