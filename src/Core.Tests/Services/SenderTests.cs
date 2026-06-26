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
        var command = new Mock<ICommand>();
        var handlerMock = new Mock<ICommandHandler<ICommand>>();
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask);

        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICommandHandler<>))))
                             .Returns(handlerMock.Object);

        await _sender.Send(command.Object, CancellationToken.None);

        handlerMock.Verify(h => h.HandleAsync(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_Command_HandlerNotRegistered_ThrowsException()
    {
        var command = new Mock<ICommand>();
        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICommandHandler<>))))
                             .Returns((object?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sender.Send(command.Object, CancellationToken.None));
    }

    [Fact]
    public async Task Send_Command_NullArgument_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sender.Send((ICommand)null!, CancellationToken.None));
    }

    [Fact]
    public async Task Send_Query_Success()
    {
        var query = new Mock<IQuery<string>>().Object;
        var handlerMock = new Mock<IQueryHandler<IQuery<string>, string>>();
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<IQuery<string>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync("Result");

        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
                             .Returns(handlerMock.Object);

        var result = await _sender.Send<string>(query, CancellationToken.None);

        Assert.Equal("Result", result);
        handlerMock.Verify(h => h.HandleAsync(It.IsAny<IQuery<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_Query_HandlerNotRegistered_ThrowsException()
    {
        var query = new Mock<IQuery<string>>();
        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
                             .Returns((object?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sender.Send<string>(query.Object, CancellationToken.None));
    }

    [Fact]
    public async Task Send_Notification_Success()
    {
        var notification = new Mock<INotification>().Object;
        var handlerMock = new Mock<INotificationHandler<INotification>>();
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask);

        var handlerEnumerable = new[] { handlerMock.Object };
        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
                             .Returns(handlerEnumerable);

        await _sender.Send(notification, CancellationToken.None);

        handlerMock.Verify(h => h.HandleAsync(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_Notification_HandlerNotRegistered_ThrowsException()
    {
        var notification = new Mock<INotification>();
        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
                             .Returns(Array.Empty<object>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sender.Send(notification.Object, CancellationToken.None));
    }

    [Fact]
    public async Task Send_CommandWithResult_Success()
    {
        var command = new Mock<ICommand<string>>();
        command.Setup(c => c.CorrelationId).Returns(Guid.NewGuid());
        var handlerMock = new Mock<ICommandHandler<ICommand<string>, string>>();
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<ICommand<string>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync("CommandResult");

        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))))
                             .Returns(handlerMock.Object);

        var result = await _sender.Send(command.Object, CancellationToken.None);

        Assert.Equal("CommandResult", result);
        handlerMock.Verify(h => h.HandleAsync(It.IsAny<ICommand<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_NotificationWithResult_Success()
    {
        var notification = new Mock<INotification<string>>();
        notification.Setup(n => n.CorrelationId).Returns(Guid.NewGuid());
        var handlerMock = new Mock<INotificationHandler<INotification<string>, string>>();
        handlerMock.Setup(h => h.HandleAsync(It.IsAny<INotification<string>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync("NotificationResult");

        _serviceProviderMock.Setup(sp => sp.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(INotificationHandler<,>))))
                             .Returns(handlerMock.Object);

        var result = await _sender.Send(notification.Object, CancellationToken.None);

        Assert.Equal("NotificationResult", result);
        handlerMock.Verify(h => h.HandleAsync(It.IsAny<INotification<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Send_ObjectGeneric_HandlerNotRegistered_ThrowsException()
    {
        var request = new object();

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sender.Send<string>(request, CancellationToken.None));
    }

    [Fact]
    public async Task Send_ObjectGeneric_NullArgument_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sender.Send<string>((object)null!, CancellationToken.None));
    }
}
