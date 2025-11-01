using OroCQRS.Core.Interfaces;

namespace CreateWebUserCommand;

public record CreateUserCommand(string Name, string Email) : ICommand
{
    public Guid CorrelationId() => Guid.NewGuid();
}