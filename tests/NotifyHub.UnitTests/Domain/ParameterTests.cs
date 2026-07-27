using FluentAssertions;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.UnitTests.Domain;

public class ParameterTests
{
    [Fact]
    public void Parameter_ThrowsDomainException_WhenOrderIsLessThanZero()
    {
        // Arrange
        int order = -1;
        string value = "Parameter";

        // Act
        Action action = () => new Parameter(order, value);

        // Assert
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Parameter_ThrowsDomainException_WhenValueIsNullOrEmpty(string? value)
    {
        // Arrange
        int order = 0;

        // Act
        Action action = () => new Parameter(order, value!);

        // Assert
        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Parameter_CreatesParameter_WhenOrderAndValueAreValid()
    {
        // Arrange
        int order = 0;
        string value = "Parameter";

        // Act
        Parameter parameter = new Parameter(order, value);

        // Assert
        parameter.Should().NotBeNull();
        parameter.Order.Should().Be(order);
        parameter.Value.Should().Be(value);
    }

    [Fact]
    public void Parameter_HasValueEquality_WhenTwoParameterCompare()
    {
        // Arrange
        Parameter parameter1 = new Parameter(0, "Parameter");
        Parameter parameter2 = new Parameter(0, "Parameter");

        // Act
        bool isEqual = parameter1 == parameter2;

        // Assert
        isEqual.Should().BeTrue();
    }
}