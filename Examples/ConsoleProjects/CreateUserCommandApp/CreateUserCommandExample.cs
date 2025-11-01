// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
using System;
using System.Threading;
using System.Threading.Tasks;
using OroCQRS.Core.Interfaces;

public record CreateUserCommand(string UserName) : ICommand
{
    public Guid CorrelationId() => Guid.NewGuid();
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    public async Task HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling CreateUserCommand for User: {command.UserName}");
        await Task.Delay(100, cancellationToken); // Simulate some work
        Console.WriteLine($"User {command.UserName} created successfully.");
    }
}