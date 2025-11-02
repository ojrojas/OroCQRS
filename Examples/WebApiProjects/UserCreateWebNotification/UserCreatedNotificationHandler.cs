using OroCQRS.Core.Interfaces;

namespace UserCreateWebNotification;

public class UserCreatedNotificationHandler : INotificationHandler<UserCreatedNotification>
{
    public Task HandleAsync(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}