using Microsoft.Extensions.Logging;
using Moq;
using OroCQRS.Core.Decorators;
using OroCQRS.Core.Interfaces;

namespace OroCQRS.Core.Tests;

public class LoggingQueryHandlerDecoratorTests
{
    [Fact]
    public async Task HandleAsync_ShouldLogMessages_CorrectFlow1()
    {
        // Arrange
        var realLogger = new Mock<ILogger<LoggingQueryHandlerDecorator<IQuery<object>, object>>>();
        var mockHandler = new Mock<IQueryHandler<IQuery<object>, object>>();
        var query = new Mock<IQuery<object>>();
        query.Setup(c => c.CorrelationId()).Returns(Guid.NewGuid());

        var decorator = new LoggingQueryHandlerDecorator<IQuery<object>, object>(realLogger.Object ,mockHandler.Object);

        // Act
        await decorator.HandleAsync(query.Object, CancellationToken.None);

        // Assert
        realLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => ContainsExpectedMessage(v, "[QUERY] IQuery`1 with CorrelationId")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Exactly(2));
    }

    [Fact]
    public async Task HandleAsync_ShouldLogMessages_CorrectFlow2()
    {
        // Arrange
        var realLogger = new Mock<ILogger<LoggingQueryHandlerDecorator<IQuery<object>, object>>>();
        var mockHandler = new Mock<IQueryHandler<IQuery<object>, object>>();
        var query = new Mock<IQuery<object>>();
        query.Setup(c => c.CorrelationId()).Returns(Guid.NewGuid());

        var decorator = new LoggingQueryHandlerDecorator<IQuery<object>, object>(realLogger.Object, mockHandler.Object);

        // Act
        await decorator.HandleAsync(query.Object, CancellationToken.None);

        // Assert
        realLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => ContainsExpectedMessage(v, "[QUERY] IQuery`1 with CorrelationId")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Exactly(2));
    }

    [Fact]
    public async Task HandleAsync_ShouldLogMessages_IncorrectFlow()
    {
        // Arrange
        var realLogger = new Mock<ILogger<LoggingQueryHandlerDecorator<IQuery<object>, object>>>();
        var mockHandler = new Mock<IQueryHandler<IQuery<object>, object>>();
        var query = new Mock<IQuery<object>>();
        query.Setup(c => c.CorrelationId()).Returns(Guid.NewGuid());

        var decorator = new LoggingQueryHandlerDecorator<IQuery<object>, object>(realLogger.Object, mockHandler.Object);

        // Act
        await decorator.HandleAsync(query.Object, CancellationToken.None);

        // Assert
        realLogger.Verify(
            x => x.Log(
                LogLevel.Error, // Incorrect log level
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v != null && v.ToString() != null && v.ToString().Contains("[QUERY] IQuery with CorrelationId")),
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