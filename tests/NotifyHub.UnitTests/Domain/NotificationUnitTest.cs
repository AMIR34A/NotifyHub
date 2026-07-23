using FluentAssertions;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.UnitTests.Domain;

public class NotificationUnitTest
{
    [Fact]
    public void Create_ThrowsDomainException_WhenMessageIsNull()
    {
        // Arrange
        var channel = Channel.Sms;
        Message? message = null;
        var parameters = new List<Parameter>();
        var data = @"""{
                                Receiver = ""
                              }""";
        var requestedBy = "UnitTest";

        // Act & Assert
        Assert.Throws<DomainException>(() => Notification.Create(channel, message!, parameters, data, requestedBy));
    }

    [Fact]
    public void Create_ThrowsDomainException_WhenMessageNeedsParameters()
    {
        // Arrange
        var channel = Channel.Sms;
        Message? message = new Message("Test template with {0}");
        var parameters = new List<Parameter>();
        var data = @"""{
                                Receiver = ""
                              }""";
        var requestedBy = "UnitTest";

        // Act & Assert
        Assert.Throws<DomainException>(() => Notification.Create(channel, message!, parameters, data, string.Join("", requestedBy)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_ThrowsDomainException_WhenDataIsNullOrEmpty(string? data)
    {
        // Arrange
        var channel = Channel.Sms;
        Message? message = new Message("Test template");
        var parameters = new List<Parameter>();
        var requestedBy = "UnitTest";

        // Act & Assert
        Assert.Throws<DomainException>(() => Notification.Create(channel, message!, parameters, data!, requestedBy));
    }

    [Fact]
    public void Create_ThrowsDomainException_WhenRequestedByLengthIsGreaterThan25()
    {
        // Arrange
        var channel = Channel.Sms;
        Message? message = new Message("Test template");
        var parameters = new List<Parameter>();
        string data = string.Empty;
        var requestedBy = Enumerable.Repeat('A', 26);

        // Act & Assert
        Assert.Throws<DomainException>(() => Notification.Create(channel, message!, parameters, data, string.Join("", requestedBy)));
    }

    [Fact]
    public void Create_CreatesNotification_WhenNotificationCreation()
    {
        // Arrange
        var channel = Channel.Sms;
        var message = new Message("Test template with {0}");
        var parameters = new List<Parameter>() { new Parameter(0, "Parameter") };
        var data = @"""{
                                Receiver = ""
                              }""";
        var requestedBy = "UnitTest";

        // Act
        var notification = Notification.Create(channel, message, parameters, data, requestedBy);

        // Assert
        notification.Channel.Should().Be(channel);
        notification.Status.Should().Be(Status.InQueue);
        notification.Message.Should().Be(message);
        notification.Parameters.Should().BeEqualTo(parameters);
        notification.Data.Should().Be(data);
        notification.RequestedBy.Should().Be(requestedBy);
        notification.RequestedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.MaxValue);
    }
}