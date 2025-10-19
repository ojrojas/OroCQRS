// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
using System;
using System.Threading;
using System.Threading.Tasks;
using OroCQRS.Core.Interfaces;

public class UserCreatedNotification : INotification
{
    public string UserName { get; set; }
    public int UserId { get; set; }
    public Guid CorrelationId { get; } = Guid.NewGuid();

    public UserCreatedNotification()
    {
        UserName = string.Empty;
    }
}

public class UserCreatedNotificationHandler : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification)
    {
        Console.WriteLine($"Notification: User {notification.UserName} was created.");
        return Task.CompletedTask;
    }

    public async Task HandleAsync(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling notification for User Created: {notification.UserName}");
        await Task.Delay(100, cancellationToken); // Simulate some work
        Console.WriteLine($"Notification handled for User Created: {notification.UserName}");
    }
}