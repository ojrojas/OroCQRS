using OroCQRS.Core.Interfaces;

namespace CreateWebUserCommand;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    public Task HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"User created: {command.Name}, {command.Email}");
        return Task.CompletedTask;
    }
}