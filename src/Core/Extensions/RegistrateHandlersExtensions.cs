// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Extensions;

public static class RegistrateHandlersExtensions
{
    /// <summary>
    /// Registers CQRS (Command Query Responsibility Segregation) handlers into the
    /// dependency injection container. Scans provided assemblies or the current AppDomain assemblies when none are specified.
    /// </summary>
    /// <param name="services">The service collection to register handlers into.</param>
    /// <param name="assembliesToScan">Optional set of assemblies to scan for handlers. If empty, all loaded assemblies will be scanned.</param>
    /// <returns>The original service collection for chaining.</returns>
    public static IServiceCollection AddCqrsHandlers(this IServiceCollection services, params Assembly[]? assembliesToScan)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Register Sender as scoped to align with typical request-scoped handlers.
        services.AddScoped<ISender, Sender>();

        var handlerInterfaceTypes = new[] {
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(INotificationHandler<>),
            typeof(INotificationHandler<,>),
            typeof(IQueryHandler<,>)
        };

        var assemblies = (assembliesToScan != null && assembliesToScan.Length > 0)
            ? assembliesToScan
            : AppDomain.CurrentDomain.GetAssemblies();

        var types = assemblies
            .Where(a => !a.IsDynamic)
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes().Where(t => t != null)!;
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null)!;
                }
            })
            .Where(t => t != null)
            .ToArray();

        var registrations = new List<(Type Service, Type Implementation)>();

        foreach (var iface in handlerInterfaceTypes)
        {
            var matches = types
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == iface)
                    .Select(i => (Service: i, Implementation: type)));

            registrations.AddRange(matches);
        }

        // Register discovered handlers. Duplicate registrations are ignored by grouping.
        foreach (var reg in registrations.Distinct())
        {
            services.AddScoped(reg.Service, reg.Implementation);
        }

        return services;
    }
}
