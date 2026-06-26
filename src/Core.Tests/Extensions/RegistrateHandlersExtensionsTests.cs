using Microsoft.Extensions.DependencyInjection;
using OroCQRS.Core.Extensions;
using OroCQRS.Core.Interfaces;

namespace OroCQRS.Core.Tests.Extensions;

public class RegistrateHandlersExtensionsTests
{
    public class TestCommand : ICommand
    {
        public Guid CorrelationId => Guid.NewGuid();
    }

    public class TestCommandWithResult : ICommand<string>
    {
        public Guid CorrelationId => Guid.NewGuid();
    }

    public class TestNotification : INotification
    {
        public Guid CorrelationId => Guid.NewGuid();
    }

    public class TestNotificationWithResult : INotification<string>
    {
        public Guid CorrelationId => Guid.NewGuid();
    }

    public class TestQuery : IQuery<string>
    {
        public Guid CorrelationId => Guid.NewGuid();
    }

    [Fact]
    public void AddCqrsHandlers_ShouldRegisterCommandHandlers()
    {
        var services = new ServiceCollection();
        services.AddCqrsHandlers();
        var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<ICommandHandler<TestCommand>>());
        Assert.NotNull(provider.GetService<ICommandHandler<TestCommandWithResult, string>>());
    }

    [Fact]
    public void AddCqrsHandlers_ShouldRegisterNotificationHandlers()
    {
        var services = new ServiceCollection();
        services.AddCqrsHandlers();
        var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<INotificationHandler<TestNotification>>());
        Assert.NotNull(provider.GetService<INotificationHandler<TestNotificationWithResult, string>>());
    }

    [Fact]
    public void AddCqrsHandlers_ShouldRegisterQueryHandlers()
    {
        var services = new ServiceCollection();
        services.AddCqrsHandlers();
        var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<IQueryHandler<TestQuery, string>>());
    }

    [Fact]
    public void AddCqrsHandlers_ShouldRegisterSender()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddCqrsHandlers();
        var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<ISender>());
    }

    [Fact]
    public void AddCqrsHandlers_WithAssemblies_ShouldRegisterHandlers()
    {
        var services = new ServiceCollection();
        services.AddCqrsHandlers(typeof(RegistrateHandlersExtensionsTests).Assembly);
        var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<ICommandHandler<TestCommand>>());
        Assert.NotNull(provider.GetService<INotificationHandler<TestNotification>>());
    }

    [Fact]
    public void AddCqrsHandlers_NullServices_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ((IServiceCollection)null!).AddCqrsHandlers());
    }

    private class MockCommandHandler : ICommandHandler<TestCommand>
    {
        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private class MockCommandHandlerWithResult : ICommandHandler<TestCommandWithResult, string>
    {
        public Task<string> HandleAsync(TestCommandWithResult command, CancellationToken cancellationToken = default) => Task.FromResult("Result");
    }

    private class MockNotificationHandler : INotificationHandler<TestNotification>
    {
        public Task HandleAsync(TestNotification notification, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private class MockNotificationHandlerWithResult : INotificationHandler<TestNotificationWithResult, string>
    {
        public Task<string> HandleAsync(TestNotificationWithResult notification, CancellationToken cancellationToken = default) => Task.FromResult("Result");
    }

    private class MockQueryHandler : IQueryHandler<TestQuery, string>
    {
        public Task<string> HandleAsync(TestQuery query, CancellationToken cancellationToken = default) => Task.FromResult("QueryResult");
    }
}
