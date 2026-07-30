using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NotifyHub.Core.ApplicationService.Notifications.Commands.Create;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Core.RequestResponse.Notifications.Commands.Create;
using System.Linq.Expressions;
using ApplicationException = NotifyHub.Core.Domain.Exceptions.ApplicationException;

namespace NotifyHub.UnitTests.Application;

public class CreateNotificationCommandHandlerTests
{
    private readonly CreateNotificationCommandHandler _sut;
    private readonly Mock<INotificationRepository> _notificationRepositoryMock;

    public CreateNotificationCommandHandlerTests()
    {
        _notificationRepositoryMock = new Mock<INotificationRepository>();
        _sut = new CreateNotificationCommandHandler(_notificationRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ThrowsApplicationException_WhenNotificationIsDuplicated()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        _notificationRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Notification, bool>>>())).ReturnsAsync(true);

        CreateNotificationCommand request = new(
            id,
            Channel.Email,
            new Message("Message"),
            new List<Parameter>(),
            @"{Receiver = "" }",
            "TestService"
        );

        // Act
        Func<Task> func = async () => await _sut.Handle(request, CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<ApplicationException>();
        _notificationRepositoryMock.Verify(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Notification, bool>>>()), Times.Once);
        _notificationRepositoryMock.Verify(repo => repo.Insert(It.IsAny<Notification>()), Times.Never);
        _notificationRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Notification>()), Times.Never);
        _notificationRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
        _notificationRepositoryMock.Verify(repo => repo.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ThrowsDbUpdateException_WhenHasProblemInSavingChanges()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        _notificationRepositoryMock.Setup(repo => repo.CommitAsync()).ThrowsAsync(new DbUpdateException());

        CreateNotificationCommand request = new(
            id,
            Channel.Email,
            new Message("Message"),
            new List<Parameter>(),
            @"{Receiver = "" }",
            "TestService"
        );

        // Act
        Func<Task> func = async () => await _sut.Handle(request, CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<DbUpdateException>();
        _notificationRepositoryMock.Verify(repo => repo.Insert(It.IsAny<Notification>()), Times.Once);
        _notificationRepositoryMock.Verify(repo => repo.CommitAsync(), Times.Once);
    }
}