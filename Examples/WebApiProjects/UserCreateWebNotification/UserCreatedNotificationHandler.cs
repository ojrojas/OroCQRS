using OroCQRS.Core.Interfaces;

namespace UserCreateWebNotification;

public class UserCreatedNotificationHandler : INotificationHandler<UserCreatedNotification>
{
    public Task HandleAsync(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Notification: User created - {notification.Name}, {notification.Email}");
        return Task.CompletedTask;
    }
}