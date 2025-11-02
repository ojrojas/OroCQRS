using OroCQRS.Core.Interfaces;

namespace UserCreateWebNotification;

public record UserCreatedNotification(string Name, string Email) : INotification
{
    public Guid CorrelationId() => Guid.NewGuid();
}