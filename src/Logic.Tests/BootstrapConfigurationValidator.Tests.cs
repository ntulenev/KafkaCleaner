using Xunit;

using FluentAssertions;

using Microsoft.Extensions.Options;

using Logic.Configuration.Validation;
using Logic.Configuration;

namespace Logic.Tests;

public class BootstrapConfigurationValidatorTests
{
    [Fact(DisplayName = "BootstrapConfigurationValidator can be created.")]
    [Trait("Category", "Unit")]
    public void CanCreateBootstrapConfigurationValidator()
    {
        // Act
        var exception = Record.Exception(() => new BootstrapConfigurationValidator());

        // Assert
        exception.Should().BeNull();
    }

    private static readonly string[] sourceArray = new[]
             {
                 "test"
             };

    [Fact(DisplayName = "BootstrapConfigurationValidator success on valid params.")]
    [Trait("Category", "Unit")]
    public void CanSuccessValidate()
    {

        // Arrange
        var validator = new BootstrapConfigurationValidator();
        var options = new BootstrapConfiguration()
        {
            BootstrapServers = [.. sourceArray]
        };

        // Act
        ValidateOptionsResult result = validator.Validate(string.Empty, options);

        // Assert
        result.Should().Be(ValidateOptionsResult.Success);
    }

    [Fact(DisplayName = "BootstrapConfigurationValidator fails on null BootstrapServers params.")]
    [Trait("Category", "Unit")]
    public void CanFailValidateNullBootstrapServers()
    {

        // Arrange
        var validator = new BootstrapConfigurationValidator();
        var options = new BootstrapConfiguration()
        {
            BootstrapServers = null!
        };

        // Act
        ValidateOptionsResult result = validator.Validate(string.Empty, options);

        // Assert
        result.Should().NotBe(ValidateOptionsResult.Success);
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = "BootstrapConfigurationValidator fails on empty BootstrapServers params.")]
    [Trait("Category", "Unit")]
    public void CanFailValidateEmptyBootstrapServers()
    {

        // Arrange
        var validator = new BootstrapConfigurationValidator();
        var options = new BootstrapConfiguration()
        {
            BootstrapServers = []
        };

        // Act
        ValidateOptionsResult result = validator.Validate(string.Empty, options);

        // Assert
        result.Should().NotBe(ValidateOptionsResult.Success);
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = "BootstrapConfigurationValidator fails on empty string in BootstrapServers params.")]
    [Trait("Category", "Unit")]
    public void CanFailValidateEmptyStringInBootstrapServers()
    {

        // Arrange
        var validator = new BootstrapConfigurationValidator();
        var options = new BootstrapConfiguration()
        {
            BootstrapServers =
            [
                "test",
                string.Empty
            ]
        };

        // Act
        ValidateOptionsResult result = validator.Validate(string.Empty, options);

        // Assert
        result.Should().NotBe(ValidateOptionsResult.Success);
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = "BootstrapConfigurationValidator fails on null string in BootstrapServers params.")]
    [Trait("Category", "Unit")]
    public void CanFailValidateNullStringInBootstrapServers()
    {

        // Arrange
        var validator = new BootstrapConfigurationValidator();
        var options = new BootstrapConfiguration()
        {
            BootstrapServers =
            [
                "test",
                null!
            ]
        };

        // Act
        ValidateOptionsResult result = validator.Validate(string.Empty, options);

        // Assert
        result.Should().NotBe(ValidateOptionsResult.Success);
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = "BootstrapConfigurationValidator fails on string with whitespaces in BootstrapServers params.")]
    [Trait("Category", "Unit")]
    public void CanFailValidateWhitespaceStringInBootstrapServers()
    {

        // Arrange
        var validator = new BootstrapConfigurationValidator();
        var options = new BootstrapConfiguration()
        {
            BootstrapServers =
            [
                "test",
                "     "
            ]
        };

        // Act
        ValidateOptionsResult result = validator.Validate(string.Empty, options);

        // Assert
        result.Should().NotBe(ValidateOptionsResult.Success);
        result.Failed.Should().BeTrue();
    }
}
