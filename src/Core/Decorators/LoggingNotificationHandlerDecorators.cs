// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Decorators;

public class LoggingNotificationHandlerDecorator<TNotification>
(ILogger<LoggingNotificationHandlerDecorator<TNotification>> logger,
INotificationHandler<TNotification> innerHandler) : INotificationHandler<TNotification> where TNotification : INotification
{
    public async Task HandleAsync(TNotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[NOTIFICATION] {typeof(TNotification).Name} with CorrelationId: {notification.CorrelationId}");
        await innerHandler.HandleAsync(notification, cancellationToken);
        logger.LogInformation($"[NOTIFICATION] {typeof(TNotification).Name} with CorrelationId: {notification.CorrelationId}");
    }
}

public class LoggingNotificationHandlerDecorator<TNotification, TResult>
(ILogger<LoggingNotificationHandlerDecorator<TNotification, TResult>> logger,
INotificationHandler<TNotification, TResult> innerHandler)
: INotificationHandler<TNotification, TResult> where TNotification : INotification<TResult>
{
    public async Task<TResult> HandleAsync(TNotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[NOTIFICATION] {typeof(TNotification).Name} with CorrelationId: {notification.CorrelationId}");
        var result = await innerHandler.HandleAsync(notification, cancellationToken);
        logger.LogInformation($"[NOTIFICATION] {typeof(TNotification).Name} with CorrelationId: {notification.CorrelationId}");
        return result;
    }
}