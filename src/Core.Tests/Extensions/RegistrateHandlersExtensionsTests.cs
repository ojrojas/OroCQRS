using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using OroCQRS.Core.Decorators;
using OroCQRS.Core.Extensions;
using OroCQRS.Core.Interfaces;

namespace OroCQRS.Core.Tests.Extensions;

public class RegistrateHandlersExtensionsTests
{
    // Clases de prueba con CorrelationId
    public class TestCommand : ICommand {
        public Guid CorrelationId () => Guid.NewGuid();
    }

    public class TestCommandWithResult : ICommand<string> {
        public Guid CorrelationId () => Guid.NewGuid();
    }

    public class TestNotification : INotification {
        public Guid CorrelationId () => Guid.NewGuid();
    }

    public class TestNotificationWithResult : INotification<string> {
        public Guid CorrelationId () => Guid.NewGuid();
    }

    [Fact]
    public void AddCqrsHandlers_ShouldRegisterCommandHandlers()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ICommandHandler<TestCommand>, MockCommandHandler>();
        services.AddSingleton<ICommandHandler<TestCommandWithResult, string>, MockCommandHandlerWithResult>();

        // Act
        services.AddCqrsHandlers();
        var provider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(provider.GetService<ICommandHandler<TestCommand>>());
        Assert.NotNull(provider.GetService<ICommandHandler<TestCommandWithResult, string>>());
    }

    [Fact]
    public void AddCqrsHandlers_ShouldRegisterNotificationHandlers()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<INotificationHandler<TestNotification>, MockNotificationHandler>();
        services.AddSingleton<INotificationHandler<TestNotificationWithResult, string>, MockNotificationHandlerWithResult>();

        // Act
        services.AddCqrsHandlers();
        var provider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(provider.GetService<INotificationHandler<TestNotification>>());
        Assert.NotNull(provider.GetService<INotificationHandler<TestNotificationWithResult, string>>());
    }

    [Fact]
    public void AddCqrsHandlers_ShouldApplyLoggingDecorators()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ICommandHandler<TestCommand>, MockCommandHandler>();
        services.AddSingleton<INotificationHandler<TestNotification>, MockNotificationHandler>();
        services.AddSingleton(Mock.Of<ILogger<LoggingCommandHandlerDecorator<TestCommand>>>());
        services.AddSingleton(Mock.Of<ILogger<LoggingNotificationHandlerDecorator<TestNotification>>>());

        // Act
        services.AddCqrsHandlers();
        var provider = services.BuildServiceProvider();

        // Assert
        Assert.NotNull(provider.GetService<ICommandHandler<TestCommand>>());
        Assert.NotNull(provider.GetService<INotificationHandler<TestNotification>>());
    }

    // Mock implementations
    private class MockCommandHandler : ICommandHandler<TestCommand>
    {
        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private class MockCommandHandlerWithResult : ICommandHandler<TestCommandWithResult, string>
    {
        public ValueTask<string> HandleAsync(TestCommandWithResult command, CancellationToken cancellationToken = default) => new ValueTask<string>("Result");
    }

    private class MockNotificationHandler : INotificationHandler<TestNotification>
    {
        public Task HandleAsync(TestNotification notification, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private class MockNotificationHandlerWithResult : INotificationHandler<TestNotificationWithResult, string>
    {
        public ValueTask<string> HandleAsync(TestNotificationWithResult notification, CancellationToken cancellationToken = default) => new ValueTask<string>("Result");
    }
}