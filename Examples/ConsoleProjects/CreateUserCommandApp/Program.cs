// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OroCQRS.Core.Extensions;
using OroCQRS.Core.Interfaces;

namespace CreateUserCommandApp
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

            services.AddCqrsHandlers();
            var provider = services.BuildServiceProvider();

            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("Application started.");

            var command = new CreateUserCommand { UserName = "Alice" };
            var handler = provider.GetRequiredService<ICommandHandler<CreateUserCommand>>();
            await handler.HandleAsync(command, CancellationToken.None);
        }
    }
}