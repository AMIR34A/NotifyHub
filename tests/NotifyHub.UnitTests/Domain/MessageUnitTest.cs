using FluentAssertions;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.UnitTests.Domain;

public class MessageUnitTest
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Message_ThrowsDomainException_WhenValueIsNullOrEmpty(string? value)
    {
        // Arrange

        // Act & Assert
        Assert.Throws<DomainException>(() => new Message(value!));
    }

    [Fact]
    public void Message_ThrowsDomainException_WhenValueLengthIsGreaterThan1500()
    {
        // Arrange
        var message = string.Join("", Enumerable.Repeat('A', 1501));

        // Act & Assert
        Assert.Throws<DomainException>(() => new Message(message));
    }

    [Fact]
    public void NeedsParameters_ReturnsTrue_WhenValueHasParameterPattern()
    {
        // Arrange
        Message message = new Message("A Message with two {0} and {1} parameters pattern");

        // Act && Assert
        message.NeedsParameters.Should().BeTrue();
    }

    [Fact]
    public void NeedsParameters_ReturnsFalse_WhenValueDoesNotHaveParameterPattern()
    {
        // Arrange
        Message message = new Message("A Message without parameters pattern");

        // Act && Assert
        message.NeedsParameters.Should().BeFalse();
    }

    [Fact]
    public void Message_CreatesMessage_WhenMessageCreation()
    {
        // Arrange
        string messageStr = "A test Message";
        // Act
        Message message = new Message(messageStr);

        // Assert
        message.Value.Should().Be(messageStr);
    }

    [Fact]
    public void Message_HasValueEquality_WhenTwoMessagesCompare()
    {
        // Arrange
        Message message1 = new Message("Message");
        Message message2 = new Message("Message");

        // Act
        bool isEqual = message1 == message2;

        // Assert
        isEqual.Should().BeTrue();
    }
}