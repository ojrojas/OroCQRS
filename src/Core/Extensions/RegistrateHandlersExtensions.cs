// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Extensions;

public static class RegistrateHandlersExtensions
{
    /// <summary>
    /// Registers CQRS (Command Query Responsibility Segregation) handlers and their decorators into the 
    /// dependency injection container. This method scans assemblies for implementations of command, query, 
    /// and notification handlers, registers them with appropriate lifetimes, and applies logging decorators 
    /// to enhance functionality.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the handlers and decorators will be added.</param>
    /// <remarks>
    /// This method performs the following tasks:
    /// <list type="bullet">
    /// <item><description>Registers logging services with a console logger and a minimum log level of Information.</description></item>
    /// <item><description>Scans the entry assembly and all loaded assemblies for implementations of <c>ICommandHandler</c>, 
    /// <c>IQueryHandler</c>, and <c>INotificationHandler</c>, and registers them as scoped services.</description></item>
    /// <item><description>Ensures all handlers are registered before applying decorators to avoid missing service issues.</description></item>
    /// <item><description>Applies logging decorators to command, query, and notification handlers safely, ensuring that 
    /// decorators are only applied if the base service exists and is not already decorated.</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// To use this method, call it during the service configuration phase in your application:
    /// <code>
    /// public void ConfigureServices(IServiceCollection services)
    /// {
    ///     services.AddCqrsHandlers();
    /// }
    /// </code>
    /// </example>
    public static void AddCqrsHandlers(this IServiceCollection services)
    {
        services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                });

        // Debugging: Log registration process
        Console.WriteLine("Registering CQRS Handlers...");

        var assembly = Assembly.GetCallingAssembly();
        services.AddTransient<ISender, Sender>();

        var handlerInterfaceTypes = new[] {
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(INotificationHandler<>),
            typeof(INotificationHandler<,>),
            typeof(IQueryHandler<,>)
        };

        var types = assembly.GetTypes();

        var handlerTypes = handlerInterfaceTypes
            .SelectMany(interfaceType => GetHandlerTypes(types, interfaceType))
            .ToList();

        foreach (var handler in handlerTypes)
        {
            services.AddScoped((Type)handler.Interface, (Type)handler.Implementation);
        }
    }

    /// Retrieves a collection of handler types that implement a specified generic handler interface type.
    /// </summary>
    /// <param name="types">An array of <see cref="Type"/> objects to search for handler implementations.</param>
    /// <param name="handlerInterfaceType">The generic handler interface type to match against.</param>
    /// <returns>
    /// An enumerable collection of dynamic objects, where each object contains:
    /// <list type="bullet">
    /// <item><description><c>Interface</c>: The generic handler interface type implemented by the class.</description></item>
    /// <item><description><c>Implementation</c>: The concrete class implementing the handler interface.</description></item>
    /// </list>
    /// </returns>
    private static IEnumerable<dynamic> GetHandlerTypes(Type[] types, Type handlerInterfaceType)
    {
        return types
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type => type.GetInterfaces()
                .Where(inter => inter.IsGenericType && inter.GetGenericTypeDefinition() == handlerInterfaceType)
                .Select(i => new { Interface = i, Implementation = type }));
    }
}