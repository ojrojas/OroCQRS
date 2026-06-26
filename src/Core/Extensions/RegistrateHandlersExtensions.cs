// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Extensions;

/// <summary>
/// Provides extension methods for registering CQRS handlers into the dependency injection container.
/// </summary>
public static class RegistrateHandlersExtensions
{
    private static readonly Type[] HandlerInterfaceTypes =
    [
        typeof(ICommandHandler<>),
        typeof(ICommandHandler<,>),
        typeof(INotificationHandler<>),
        typeof(INotificationHandler<,>),
        typeof(IQueryHandler<,>)
    ];

    /// <summary>
    /// Registers CQRS (Command Query Responsibility Segregation) handlers into the
    /// dependency injection container. Scans the specified assemblies (or all loaded assemblies when none are specified).
    /// </summary>
    /// <param name="services">The service collection to register handlers into.</param>
    /// <param name="assembliesToScan">
    /// Optional set of assemblies to scan for handlers. If omitted, all assemblies in the current AppDomain are scanned.
    /// It is recommended to explicitly specify assemblies for better performance and predictability.
    /// </param>
    /// <returns>The original service collection for chaining.</returns>
    public static IServiceCollection AddCqrsHandlers(this IServiceCollection services, params Assembly[]? assembliesToScan)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddScoped<ISender, Sender>();

        var assemblies = (assembliesToScan != null && assembliesToScan.Length > 0)
            ? assembliesToScan
            : AppDomain.CurrentDomain.GetAssemblies();

        var types = assemblies
            .Where(a => !a.IsDynamic)
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types;
                }
            })
            .OfType<Type>()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToArray();

        var registrations = new List<(Type Service, Type Implementation)>();

        foreach (var iface in HandlerInterfaceTypes)
        {
            var matches = types
                .SelectMany(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == iface)
                    .Select(i => (Service: i, Implementation: type)));

            registrations.AddRange(matches);
        }

        foreach (var (Service, Implementation) in registrations.Distinct())
        {
            services.AddScoped(Service, Implementation);
        }

        return services;
    }
}
