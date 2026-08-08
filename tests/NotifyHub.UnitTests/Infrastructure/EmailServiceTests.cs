using FluentAssertions;
using Moq;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Infrastructure.Services.Emails;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.UnitTests.Infrastructure;

public class EmailServiceTests
{
    private readonly EmailService _sut;
    private readonly Mock<IEmailProvider> _emailServiceProviderMock = new Mock<IEmailProvider>();
    private readonly Mock<IJsonSerializerService> _jsonSerializerServiceMock = new Mock<IJsonSerializerService>();

    public EmailServiceTests()
    {
        _sut = new EmailService([_emailServiceProviderMock.Object], _jsonSerializerServiceMock.Object);
    }

    [Fact]
    public void EmailService_HasChannelProperty_ReturnsEmailChannel()
    {
        // Arrange

        // Act
        var channel = _sut.Channel;

        // Assert
        channel.Should().Be(Channel.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task SendAsync_ThrowsServiceException_WhenPayloadIsNullOrEmpty(string? payload)
    {
        // Arrange

        // Act
        Func<Task> func = async () => await _sut.SendAsync("Message", payload!, CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<ServiceException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task SendAsync_ThrowsServiceException_WhenMessageIsNullOrEmpty(string? message)
    {
        // Arrange

        // Act
        Func<Task> func = async () => await _sut.SendAsync(message!, "Payload", CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<ServiceException>();
    }

    [Fact]
    public async Task SendAsync_ThrowsServiceException_WhenCanNotDeserializePayload()
    {
        // Arrange
        _jsonSerializerServiceMock.Setup(s => s.Deserialize<EmailPayload>(It.IsAny<string>()))
            .Returns(() => null);

        // Act
        Func<Task> func = async () => await _sut.SendAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<ServiceException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task SendAsync_ThrowsServiceException_WhenPayloadReceiverIsNullOrEmpty(string? receiver)
    {
        // Arrange
        _jsonSerializerServiceMock.Setup(s => s.Deserialize<EmailPayload>(It.IsAny<string>()))
            .Returns(() => new EmailPayload(receiver!, "Email Subject"));

        // Act
        Func<Task> func = async () => await _sut.SendAsync("Message", "Payload", CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<ServiceException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task SendAsync_ThrowsServiceException_WhenPayloadSubjectIsNullOrEmpty(string? subject)
    {
        // Arrange
        _jsonSerializerServiceMock.Setup(s => s.Deserialize<EmailPayload>(It.IsAny<string>()))
            .Returns(() => new EmailPayload("test@gmail.com", subject!));

        // Act
        Func<Task> func = async () => await _sut.SendAsync("Message", "Payload", CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<ServiceException>();
    }

    [Fact]
    public async Task SendAsync_ReturnsFalse_WhenAllEmailProvidersDoNotRespond()
    {
        // Arrange
        _emailServiceProviderMock.Setup(p => p.SendAsync(It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(OperationResult.Fail(ErrorType.Unexpected, []));

        _jsonSerializerServiceMock.Setup(s => s.Deserialize<EmailPayload>(It.IsAny<string>()))
            .Returns(() => new EmailPayload("Receiver", "Subject"));

        // Act
        bool result = await _sut.SendAsync("Message", "Payload", CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _emailServiceProviderMock.Verify(p => p.SendAsync(It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendAsync_DoesNotThrowAnyExceptions_WhenAnEmailProviderResponds()
    {
        // Arrange
        _emailServiceProviderMock.Setup(p => p.SendAsync(It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(OperationResult.Succuss());

        _jsonSerializerServiceMock.Setup(s => s.Deserialize<EmailPayload>(It.IsAny<string>()))
            .Returns(() => new EmailPayload("Receiver", "Subject"));

        // Act
        bool result = await _sut.SendAsync("Message", "Payload", CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _emailServiceProviderMock.Verify(p => p.SendAsync(It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<CancellationToken>()), Times.Once);
    }
}