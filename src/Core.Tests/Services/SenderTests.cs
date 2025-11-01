using Microsoft.Extensions.Logging;
using Moq;
using OroCQRS.Core.Interfaces;
using OroCQRS.Core.Services;

namespace OroCQRS.Core.Tests.Services;

public class SenderTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<ILogger<Sender>> _loggerMock;
    private readonly Sender _sender;

    public SenderTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _loggerMock = new Mock<ILogger<Sender>>();
        _sender = new Sender(_loggerMock.Object, _serviceProviderMock.Object);
    }

    [Fact]
    public async Task Send_Command_Success()
    {
        // Arrange
        var command = new Mock<ICommand>().Object;
        var handlerMock = new Mock<ICommandHandler<ICommand>>();
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask);

        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICommandHandler<>))))
                             .Returns(handlerMock.Object);

        // Act
        await _sender.Send(command, CancellationToken.None);

        // Assert
        handlerMock.Verify(h => h.HandleAsync(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_Command_HandlerNotRegistered_ThrowsException()
    {
        // Arrange
        var command = new Mock<ICommand>();
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(ICommandHandler<ICommand>)))
                             .Returns((object?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sender.Send(command.Object, CancellationToken.None));
    }

    [Fact]
    public async Task Send_Query_Success()
    {
        // Arrange
        var query = new Mock<IQuery<string>>().Object;
        var handlerMock = new Mock<IQueryHandler<IQuery<string>, string>>();
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<IQuery<string>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync("Result");

        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
                             .Returns(handlerMock.Object);

        // Act
        var result = await _sender.Send<string>(query, CancellationToken.None);

        // Assert
        Assert.Equal("Result", result);
        handlerMock.Verify(h => h.HandleAsync(It.IsAny<IQuery<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_Query_HandlerNotRegistered_ThrowsException()
    {
        // Arrange
        var query = new Mock<IQuery<string>>();
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(IQueryHandler<IQuery<string>, string>)))
                             .Returns((object?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sender.Send<string>(query.Object, CancellationToken.None));
    }

    [Fact]
    public async Task Send_Notification_Success()
    {
        // Arrange
        var notification = new Mock<INotification>().Object;
        var handlerMock = new Mock<INotificationHandler<INotification>>();
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask);

        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(INotificationHandler<>))))
                             .Returns(handlerMock.Object);

        // Act
        await _sender.Send(notification, CancellationToken.None);

        // Assert
        handlerMock.Verify(h => h.HandleAsync(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_Notification_HandlerNotRegistered_ThrowsException()
    {
        // Arrange
        var notification = new Mock<INotification>();
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(INotificationHandler<INotification>)))
                             .Returns((object?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sender.Send(notification.Object, CancellationToken.None));
    }
}