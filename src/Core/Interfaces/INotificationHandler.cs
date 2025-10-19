// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;
public interface INotificationHandler<in TNotification> where TNotification : INotification
{
    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}

public interface INotificationHandler<in TNotification, TResult> 
where TNotification : INotification<TResult>
{
    ValueTask<TResult> HandleAsync(
        TNotification notification, CancellationToken cancellationToken);
}