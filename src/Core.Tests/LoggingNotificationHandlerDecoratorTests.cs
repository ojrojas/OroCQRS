using Microsoft.Extensions.Logging;
using Moq;
using OroCQRS.Core.Decorators;
using OroCQRS.Core.Interfaces;

namespace OroCQRS.Core.Tests;

public class LoggingNotificationHandlerDecoratorTests
{
    [Fact]
    public async Task HandleAsync_ShouldLogMessages_CorrectFlow1()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<LoggingNotificationHandlerDecorator<INotification>>>();
        var mockHandler = new Mock<INotificationHandler<INotification>>();
        var notification = new Mock<INotification>();
        notification.Setup(c => c.CorrelationId).Returns(Guid.NewGuid());

        var decorator = new LoggingNotificationHandlerDecorator<INotification>( mockLogger.Object, mockHandler.Object);

        // Act
        await decorator.HandleAsync(notification.Object, CancellationToken.None);

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => ContainsExpectedMessage(v, "[NOTIFICATION] INotification with CorrelationId")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Exactly(2));
    }

    [Fact]
    public async Task HandleAsync_ShouldLogMessages_CorrectFlow2()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<LoggingNotificationHandlerDecorator<INotification>>>();
        var mockHandler = new Mock<INotificationHandler<INotification>>();
        var notification = new Mock<INotification>();
        notification.Setup(c => c.CorrelationId).Returns(Guid.NewGuid());

        var decorator = new LoggingNotificationHandlerDecorator<INotification>( mockLogger.Object, mockHandler.Object);

        // Act
        await decorator.HandleAsync(notification.Object, CancellationToken.None);

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => ContainsExpectedMessage(v, "[NOTIFICATION] INotification with CorrelationId")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Exactly(2));
    }

    [Fact]
    public async Task HandleAsync_ShouldLogMessages_IncorrectFlow()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<LoggingNotificationHandlerDecorator<INotification>>>();
        var mockHandler = new Mock<INotificationHandler<INotification>>();
        var notification = new Mock<INotification>();
        notification.Setup(c => c.CorrelationId).Returns(Guid.NewGuid());

        var decorator = new LoggingNotificationHandlerDecorator<INotification>( mockLogger.Object, mockHandler.Object);

        // Act
        await decorator.HandleAsync(notification.Object, CancellationToken.None);

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Error, // Incorrect log level
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => ContainsExpectedMessage(v, "[NOTIFICATION] INotification with CorrelationId")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Never());
    }

    private bool ContainsExpectedMessage(object? value, string expectedMessage)
    {
        var stringValue = value?.ToString();
        if (stringValue == null)
        {
            return false;
        }
        return stringValue.Contains(expectedMessage);
    }
}