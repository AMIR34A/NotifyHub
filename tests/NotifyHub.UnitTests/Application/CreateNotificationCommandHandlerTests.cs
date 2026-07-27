using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NotifyHub.Core.ApplicationService.Notifications.Commands.Create;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Domain.Notifications;
using System.Linq.Expressions;
using ApplicationException = NotifyHub.Core.Domain.Exceptions.ApplicationException;

namespace NotifyHub.UnitTests.Application;

public class CreateNotificationCommandHandlerTests
{
    private readonly CreateNotificationCommandHandler _sut;
    private readonly Mock<INotificationRepository> notificationRepositoryMock;

    public CreateNotificationCommandHandlerTests()
    {
        notificationRepositoryMock = new Mock<INotificationRepository>();
        _sut = new CreateNotificationCommandHandler(notificationRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ThrowsApplicationException_WhenNotificationIsDuplicated()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        notificationRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Notification, bool>>>())).ReturnsAsync(true);

        CreateNotificationCommand request = new(
            id,
            Channel.Email,
            new Message("Message"),
            new List<Parameter>(),
            @"{Receiver = "" }",
            "TestService"
        );

        // Act
        Func<Task> action = async () => await _sut.Handle(request, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApplicationException>();
        notificationRepositoryMock.Verify(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Notification, bool>>>()), Times.Once);
        notificationRepositoryMock.Verify(repo => repo.Insert(It.IsAny<Notification>()), Times.Never);
        notificationRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Notification>()), Times.Never);
        notificationRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
        notificationRepositoryMock.Verify(repo => repo.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ThrowsDbUpdateException_WhenHasProblemInSavingChanges()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        notificationRepositoryMock.Setup(repo => repo.CommitAsync()).ThrowsAsync(new DbUpdateException());

        CreateNotificationCommand request = new(
            id,
            Channel.Email,
            new Message("Message"),
            new List<Parameter>(),
            @"{Receiver = "" }",
            "TestService"
        );

        // Act
        Func<Task> action = async () => await _sut.Handle(request, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<DbUpdateException>();
        notificationRepositoryMock.Verify(repo => repo.Insert(It.IsAny<Notification>()), Times.Once);
        notificationRepositoryMock.Verify(repo => repo.CommitAsync(), Times.Once);
    }
}