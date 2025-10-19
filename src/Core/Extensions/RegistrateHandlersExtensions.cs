// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Extensions;

public static class RegistrateHandlersExtensions
{
    public static void AddCqrsHandlers(this IServiceCollection services)
    {
        services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                });

        // Debugging: Log registration process
        Console.WriteLine("Registering CQRS Handlers...");

        // Register command handlers
        services.Scan(scan => scan
            .FromEntryAssembly()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
           .FromEntryAssembly()
           .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
           .AsImplementedInterfaces()
           .WithScopedLifetime());

        // Debugging: Log registration process
        Console.WriteLine("Registering CQRS Handlers...");

        // Register notifications handlers
        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(INotificationHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(INotificationHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        Console.WriteLine("Command handlers registered.");

        // Register query handlers
        services.Scan(scan => scan
            .FromEntryAssembly()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        Console.WriteLine("Query handlers registered.");

        // Ensure all handlers are registered before applying decorators
        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(INotificationHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(INotificationHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Improved decorator registration to ensure base services exist
        // Adjust SafeDecorate to handle missing services gracefully
        void SafeDecorate(Type serviceType, Type decoratorType)
        {
            if (services.Any(sd => sd.ServiceType == serviceType && sd.ImplementationType != decoratorType))
            {
                try
                {
                    services.Decorate(serviceType, decoratorType);
                    Console.WriteLine($"Applied decorator {decoratorType.Name} to {serviceType.Name}");
                }
                catch (Scrutor.DecorationException ex)
                {
                    Console.WriteLine($"Failed to apply decorator {decoratorType.Name} to {serviceType.Name}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Skipping decorator {decoratorType.Name} as no base service for {serviceType.Name} is registered or it is already decorated.");
            }
        }

        // Apply logging decorators safely
        SafeDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        SafeDecorate(typeof(ICommandHandler<,>), typeof(LoggingCommandHandlerDecorator<,>));
        SafeDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
        SafeDecorate(typeof(INotificationHandler<>), typeof(LoggingNotificationHandlerDecorator<>));
        SafeDecorate(typeof(INotificationHandler<,>), typeof(LoggingNotificationHandlerDecorator<,>));
        Console.WriteLine("Logging decorators applied.");
    }
}