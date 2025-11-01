// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OroCQRS.Core.Extensions;
using OroCQRS.Core.Interfaces;
using OroCQRS.Core.Services;

namespace GetUserQueryApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            services.AddScoped<ISender,Sender>();
            services.AddCqrsHandlers();
            var provider = services.BuildServiceProvider();

            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("Application started.");

            var query = new GetUserQuery(UserId: 1);
            var handler = provider.GetRequiredService<ISender>();
            var user = await handler.Send(query, CancellationToken.None);
            Console.WriteLine($"User fetched: {user.Name}");
        }
    }
}