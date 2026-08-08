using FluentAssertions;
using Moq;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Infrastructure.Services.SMSs;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.UnitTests.Infrastructure;

public class SmsServiceTests
{
    private readonly SmsService _sut;
    private readonly Mock<ISmsProvider> _smsServiceProviderMock = new Mock<ISmsProvider>();
    private readonly Mock<IJsonSerializerService> _jsonSerializerServiceMock = new Mock<IJsonSerializerService>();

    public SmsServiceTests()
    {
        _sut = new SmsService([_smsServiceProviderMock.Object], _jsonSerializerServiceMock.Object);
    }

    [Fact]
    public void SmsService_HasChannelProperty_ReturnsSmsChannel()
    {
        // Arrange

        // Act
        var channel = _sut.Channel;

        // Assert
        channel.Should().Be(Channel.Sms);
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
        _jsonSerializerServiceMock.Setup(s => s.Deserialize<SmsPayload>(It.IsAny<string>()))
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
        _jsonSerializerServiceMock.Setup(s => s.Deserialize<SmsPayload>(It.IsAny<string>()))
            .Returns(() => new SmsPayload(receiver!));

        // Act
        Func<Task> func = async () => await _sut.SendAsync("Message", "Payload", CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<ServiceException>();
    }

    [Fact]
    public async Task SendAsync_ReturnsFalse_WhenAllSmsProvidersDoNotRespond()
    {
        // Arrange
        _smsServiceProviderMock.Setup(p => p.SendAsync(It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(OperationResult.Fail(ErrorType.Unexpected, []));

        _jsonSerializerServiceMock.Setup(s => s.Deserialize<SmsPayload>(It.IsAny<string>()))
            .Returns(() => new SmsPayload("Receiver"));

        // Act
        bool result = await _sut.SendAsync("Message", "Payload", CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _smsServiceProviderMock.Verify(p => p.SendAsync(It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendAsync_DoesNotThrowAnyExceptions_WhenASmsProviderResponds()
    {
        // Arrange
        _smsServiceProviderMock.Setup(p => p.SendAsync(It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(OperationResult.Succuss());

        _jsonSerializerServiceMock.Setup(s => s.Deserialize<SmsPayload>(It.IsAny<string>()))
            .Returns(() => new SmsPayload("Receiver"));

        // Act
        bool result = await _sut.SendAsync("Message", "Payload", CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _smsServiceProviderMock.Verify(p => p.SendAsync(It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<CancellationToken>()), Times.Once);
    }
}