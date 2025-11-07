namespace F23.Kernel.Tests;

public class ValidationErrorExtensionsTests
{
    [Fact]
    public void CreateErrorDictionary_WithSingleError_Returns_DictionaryWithOneEntry()
    {
        // Arrange
        var errors = new[] { new ValidationError("email", "Invalid email") };

        // Act
        var result = errors.CreateErrorDictionary();

        // Assert
        Assert.Single(result);
        Assert.True(result.TryGetValue("email", out var messages));
        Assert.Single(messages);
        Assert.Equal("Invalid email", messages[0]);
    }

    [Fact]
    public void CreateErrorDictionary_WithMultipleErrorsDifferentKeys_Returns_DictionaryWithMultipleEntries()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError("email", "Invalid email"),
            new ValidationError("password", "Password too short"),
            new ValidationError("username", "Username taken")
        };

        // Act
        var result = errors.CreateErrorDictionary();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.True(result.TryGetValue("email", out var emailMessages));
        Assert.Single(emailMessages);
        Assert.Equal("Invalid email", emailMessages[0]);

        Assert.True(result.TryGetValue("password", out var passwordMessages));
        Assert.Single(passwordMessages);
        Assert.Equal("Password too short", passwordMessages[0]);

        Assert.True(result.TryGetValue("username", out var usernameMessages));
        Assert.Single(usernameMessages);
        Assert.Equal("Username taken", usernameMessages[0]);
    }

    [Fact]
    public void CreateErrorDictionary_WithMultipleErrorsSameKey_Returns_AccumulatedMessages()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError("email", "Email is required"),
            new ValidationError("email", "Email format is invalid"),
            new ValidationError("email", "Email already registered")
        };

        // Act
        var result = errors.CreateErrorDictionary();

        // Assert
        Assert.Single(result);
        Assert.True(result.TryGetValue("email", out var messages));
        Assert.Equal(3, messages.Length);
        Assert.Equal("Email is required", messages[0]);
        Assert.Equal("Email format is invalid", messages[1]);
        Assert.Equal("Email already registered", messages[2]);
    }

    [Fact]
    public void CreateErrorDictionary_WithMixedSingleAndMultipleErrors_Returns_ProperlyAccumulatedMessages()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError("email", "Email is required"),
            new ValidationError("email", "Email format is invalid"),
            new ValidationError("password", "Password too short"),
            new ValidationError("username", "Username taken"),
            new ValidationError("username", "Username contains invalid characters")
        };

        // Act
        var result = errors.CreateErrorDictionary();

        // Assert
        Assert.Equal(3, result.Count);

        Assert.True(result.TryGetValue("email", out var emailMessages));
        Assert.Equal(2, emailMessages.Length);
        Assert.Equal(new[] { "Email is required", "Email format is invalid" }, emailMessages);

        Assert.True(result.TryGetValue("password", out var passwordMessages));
        Assert.Single(passwordMessages);
        Assert.Equal("Password too short", passwordMessages[0]);

        Assert.True(result.TryGetValue("username", out var usernameMessages));
        Assert.Equal(2, usernameMessages.Length);
        Assert.Equal(new[] { "Username taken", "Username contains invalid characters" }, usernameMessages);
    }

    [Fact]
    public void CreateErrorDictionary_WithEmptyCollection_Returns_EmptyDictionary()
    {
        // Arrange
        var errors = Array.Empty<ValidationError>();

        // Act
        var result = errors.CreateErrorDictionary();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void CreateErrorDictionary_WithNullInput_Throws_ArgumentNullException()
    {
        // Arrange
        IReadOnlyCollection<ValidationError>? errors = null;

        // Act
        void Act() => errors!.CreateErrorDictionary();

        // Assert
        Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact]
    public void CreateErrorDictionary_IsCaseSensitive()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError("email", "Error 1"),
            new ValidationError("Email", "Error 2"),
            new ValidationError("EMAIL", "Error 3")
        };

        // Act
        var result = errors.CreateErrorDictionary();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.True(result.TryGetValue("email", out var lowercaseMessages));
        Assert.Single(lowercaseMessages);
        Assert.Equal("Error 1", lowercaseMessages[0]);

        Assert.True(result.TryGetValue("Email", out var pascalcaseMessages));
        Assert.Single(pascalcaseMessages);
        Assert.Equal("Error 2", pascalcaseMessages[0]);

        Assert.True(result.TryGetValue("EMAIL", out var uppercaseMessages));
        Assert.Single(uppercaseMessages);
        Assert.Equal("Error 3", uppercaseMessages[0]);
    }

    [Fact]
    public void CreateErrorDictionary_PreservesErrorOrder()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError("field", "First error"),
            new ValidationError("field", "Second error"),
            new ValidationError("field", "Third error"),
            new ValidationError("field", "Fourth error")
        };

        // Act
        var result = errors.CreateErrorDictionary();

        // Assert
        Assert.True(result.TryGetValue("field", out var messages));
        Assert.Equal(4, messages.Length);
        Assert.Equal("First error", messages[0]);
        Assert.Equal("Second error", messages[1]);
        Assert.Equal("Third error", messages[2]);
        Assert.Equal("Fourth error", messages[3]);
    }

    [Fact]
    public void CreateErrorDictionary_WithSpecialCharacterKeys_Returns_CorrectMapping()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError("user.email", "Invalid email"),
            new ValidationError("user-phone", "Invalid phone"),
            new ValidationError("user_address", "Invalid address")
        };

        // Act
        var result = errors.CreateErrorDictionary();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.True(result.ContainsKey("user.email"));
        Assert.True(result.ContainsKey("user-phone"));
        Assert.True(result.ContainsKey("user_address"));
    }
}
