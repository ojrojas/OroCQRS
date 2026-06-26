// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Services;

/// <summary>
/// Dispatches commands, queries, and notifications to their registered handlers
/// by resolving the appropriate handler interface from the service provider.
/// </summary>
/// <param name="logger">Logger for diagnostic output.</param>
/// <param name="provider">Service provider used to resolve handler instances.</param>
public class Sender(ILogger<Sender> logger, IServiceProvider provider) : ISender
{
    public async Task Send(ICommand request, CancellationToken cancellationToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        logger.LogInformation("[COMMAND] {Command} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);

        var handlerType = typeof(ICommandHandler<>).MakeGenericType(request.GetType());
        var handler = provider.GetService(handlerType);
        if (handler == null)
        {
            logger.LogWarning("No command handler registered for {CommandType}", request.GetType());
            throw new InvalidOperationException($"No handler registered for {handlerType}");
        }

        var handleMethod = handlerType.GetMethod("HandleAsync", [request.GetType(), typeof(CancellationToken)])!;
        var task = (Task)handleMethod.Invoke(handler, [request, cancellationToken])!;
        await task.ConfigureAwait(false);

        logger.LogInformation("[COMMAND] {Command} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);
    }

    public async Task<TResult> Send<TResult>(ICommand<TResult> request, CancellationToken cancellationToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        logger.LogInformation("[COMMAND] {Command} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);

        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        var handler = provider.GetService(handlerType);
        if (handler == null)
        {
            logger.LogWarning("No command handler registered for {CommandType}", request.GetType());
            throw new InvalidOperationException($"No handler registered for {handlerType}");
        }

        var handleMethod = handlerType.GetMethod("HandleAsync", [request.GetType(), typeof(CancellationToken)])!;
        var task = (Task<TResult>)handleMethod.Invoke(handler, [request, cancellationToken])!;
        var response = await task.ConfigureAwait(false);

        logger.LogInformation("[COMMAND] {Command} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);
        return response;
    }

    public async Task<TResult> Send<TResult>(IQuery<TResult> request, CancellationToken cancellationToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        logger.LogInformation("[QUERY] {Query} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);

        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        var handler = provider.GetService(handlerType);
        if (handler == null)
        {
            logger.LogWarning("No query handler registered for {QueryType}", request.GetType());
            throw new InvalidOperationException($"No handler registered for {handlerType}");
        }

        var handleMethod = handlerType.GetMethod("HandleAsync", [request.GetType(), typeof(CancellationToken)])!;
        var task = (Task<TResult>)handleMethod.Invoke(handler, [request, cancellationToken])!;
        var response = await task.ConfigureAwait(false);

        logger.LogInformation("[QUERY] {Query} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);
        return response;
    }

    public async Task Send(INotification request, CancellationToken cancellationToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        logger.LogInformation("[NOTIFICATION] {Notification} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);

        var handlerType = typeof(INotificationHandler<>).MakeGenericType(request.GetType());
        var handlers = provider.GetService(typeof(IEnumerable<>).MakeGenericType(handlerType)) as IEnumerable<object>;

        if (handlers == null || !handlers.Any())
        {
            logger.LogWarning("No notification handlers registered for {NotificationType}", request.GetType());
            throw new InvalidOperationException($"No handler registered for {handlerType}");
        }

        var handleMethod = handlerType.GetMethod("HandleAsync", [request.GetType(), typeof(CancellationToken)])!;

        foreach (var handler in handlers)
        {
            var task = (Task)handleMethod.Invoke(handler, [request, cancellationToken])!;
            await task.ConfigureAwait(false);
        }

        logger.LogInformation("[NOTIFICATION] {Notification} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);
    }

    public async Task<TResult> Send<TResult>(INotification<TResult> request, CancellationToken cancellationToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        logger.LogInformation("[NOTIFICATION] {Notification} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);

        var handlerType = typeof(INotificationHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        var handler = provider.GetService(handlerType);
        if (handler == null)
        {
            logger.LogWarning("No notification handler registered for {NotificationType}", request.GetType());
            throw new InvalidOperationException($"No handler registered for {handlerType}");
        }

        var handleMethod = handlerType.GetMethod("HandleAsync", [request.GetType(), typeof(CancellationToken)])!;
        var task = (Task<TResult>)handleMethod.Invoke(handler, [request, cancellationToken])!;
        var response = await task.ConfigureAwait(false);

        logger.LogInformation("[NOTIFICATION] {Notification} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId);
        return response;
    }

    public async Task<TResult?> Send<TResult>(object request, CancellationToken cancellationToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var correlationId = request is IBaseMessage msg ? msg.CorrelationId : Guid.NewGuid();
        logger.LogInformation("[SEND OBJECT GENERIC] {Request} with CorrelationId: {CorrelationId}", request.GetType().Name, correlationId);

        var handlerInterfacesToTry = new[] { typeof(ICommandHandler<,>), typeof(IQueryHandler<,>), typeof(INotificationHandler<,>) };

        foreach (var openHandler in handlerInterfacesToTry)
        {
            var specificHandlerType = TryMakeGenericType(openHandler, [request.GetType(), typeof(TResult)]);
            if (specificHandlerType == null)
                continue;

            var handler = provider.GetService(specificHandlerType);
            if (handler != null)
            {
                var handleMethod = specificHandlerType.GetMethod("HandleAsync", [request.GetType(), typeof(CancellationToken)])!;
                var task = (Task<TResult>)handleMethod.Invoke(handler, [request, cancellationToken])!;
                var response = await task.ConfigureAwait(false);
                logger.LogInformation("[SEND OBJECT GENERIC] {Request} with CorrelationId: {CorrelationId}", request.GetType().Name, correlationId);
                return response;
            }
        }

        logger.LogWarning("No handler found for request type {RequestType} -> {ResultType}", request.GetType(), typeof(TResult));
        throw new InvalidOperationException($"No handler registered for request type {request.GetType()} with result type {typeof(TResult)}");
    }

    private static Type? TryMakeGenericType(Type openGeneric, Type[] typeArguments)
    {
        try
        {
            return openGeneric.MakeGenericType(typeArguments);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
}
