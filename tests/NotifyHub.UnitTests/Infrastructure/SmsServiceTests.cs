using FluentAssertions;
using Moq;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Infrastructure.Services.SMSs;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Results;
using Polly;
using Polly.Retry;

namespace NotifyHub.UnitTests.Infrastructure;

public class SmsServiceTests
{
    private readonly SmsService _sut;
    private readonly Mock<ISmsProvider> _smsServiceProviderMock = new Mock<ISmsProvider>();
    private readonly Mock<IJsonSerializerService> _jsonSerializerServiceMock = new Mock<IJsonSerializerService>();
    private const int RetryCount = 1;

    public SmsServiceTests()
    {
        ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = RetryCount,
            })
            .Build();

        _sut = new SmsService([_smsServiceProviderMock.Object], _jsonSerializerServiceMock.Object, pipeline);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task SendAsync_ThrowsServiceException_WhenPayloadIsNullOrEmpty(string? payload)
    {
        // Arrange

        // Act
        Func<Task> func = async () => await _sut.SendAsync(payload!);

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
        Func<Task> func = async () => await _sut.SendAsync(It.IsAny<string>());

        // Assert
        await func.Should().ThrowAsync<ServiceException>();
    }

    [Fact]
    public async Task SendAsync_ThrowsServiceException_WhenAllSmsProvidersDoNotRespond()
    {
        // Arrange
        _smsServiceProviderMock.Setup(p => p.SendAsync(It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(OperationResult.Fail(ErrorType.Unexpected, []));

        _jsonSerializerServiceMock.Setup(s => s.Deserialize<SmsPayload>(It.IsAny<string>()))
            .Returns(() => new SmsPayload("", ""));

        // Act
        Func<Task> func = async () => await _sut.SendAsync("Payload");

        // Assert
        await func.Should().ThrowAsync<ServiceException>();
        _smsServiceProviderMock.Verify(p => p.SendAsync(It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Exactly(RetryCount + 1));
    }

    [Fact]
    public async Task SendAsync_DoesNotThrowAnyExceptions_WhenASmsProviderResponds()
    {
        // Arrange
        _smsServiceProviderMock.Setup(p => p.SendAsync(It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(OperationResult.Succuss());

        _jsonSerializerServiceMock.Setup(s => s.Deserialize<SmsPayload>(It.IsAny<string>()))
            .Returns(() => new SmsPayload("", ""));

        // Act
        Func<Task> func = async () => await _sut.SendAsync("Payload");

        // Assert
        await func.Should().NotThrowAsync<Exception>();
        _smsServiceProviderMock.Verify(p => p.SendAsync(It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<CancellationToken>()), Times.Once);
    }
}