using Microsoft.Extensions.Logging;
using Moq;
using OroCQRS.Core.Decorators;
using OroCQRS.Core.Interfaces;

namespace OroCQRS.Core.Tests;

public class LoggingCommandHandlerDecoratorTests
{
    [Fact]
    public async Task HandleAsync_ShouldLogMessages_CorrectFlow()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<LoggingCommandHandlerDecorator<ICommand>>>();
        var mockHandler = new Mock<ICommandHandler<ICommand>>();
        var command = new Mock<ICommand>();
        command.Setup(c => c.CorrelationId).Returns(Guid.NewGuid());

        var decorator = new LoggingCommandHandlerDecorator<ICommand>(mockLogger.Object, mockHandler.Object);

        // Act
        await decorator.HandleAsync(command.Object, CancellationToken.None);

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => ContainsExpectedMessage(v, "[COMMAND] ICommand with CorrelationId")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Exactly(2));
    }

    [Fact]
    public async Task HandleAsync_ShouldLogMessages_IncorrectFlow()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<LoggingCommandHandlerDecorator<ICommand>>>();
        var mockHandler = new Mock<ICommandHandler<ICommand>>();
        var command = new Mock<ICommand>();
        command.Setup(c => c.CorrelationId).Returns(Guid.NewGuid());

        var decorator = new LoggingCommandHandlerDecorator<ICommand>(mockLogger.Object, mockHandler.Object);

        // Act
        await decorator.HandleAsync(command.Object, CancellationToken.None);

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Error, // Incorrect log level
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => (v as string).ToString().Contains("[COMMAND] ICommand with CorrelationId") == true),
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