// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Services;

public class Sender(ILogger<Sender> logger, IServiceProvider provider) : ISender
{
    public async Task Send(ICommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("[COMMAND] {Command} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());

        var handlerType = typeof(ICommandHandler<>).MakeGenericType(request.GetType());
        var handler = provider.GetService(handlerType);
        if (handler == null)
        {
            logger.LogWarning("No command handler registered for {CommandType}", request.GetType());
            throw new InvalidOperationException($"No handler registered for {handlerType}");
        }

        dynamic dynHandler = handler;
        await dynHandler.HandleAsync((dynamic)request, cancellationToken);

        logger.LogInformation("[COMMAND] {Command} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());
    }

    public async Task<TResult> Send<TResult>(ICommand<TResult> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("[COMMAND] {Command} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());

        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        var handler = provider.GetService(handlerType);
        if (handler == null)
        {
            logger.LogWarning("No command handler registered for {CommandType}", request.GetType());
            throw new InvalidOperationException($"No handler registered for {handlerType}");
        }

        dynamic dynHandler = handler;
        var response = await dynHandler.HandleAsync((dynamic)request, cancellationToken);

        logger.LogInformation("[COMMAND] {Command} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());
        return (TResult)response!;
    }

    public async Task<TResult> Send<TResult>(IQuery<TResult> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("[QUERY] {Query} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());

        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        var handler = provider.GetService(handlerType);
        if (handler == null)
        {
            logger.LogWarning("No query handler registered for {QueryType}", request.GetType());
            throw new InvalidOperationException($"No handler registered for {handlerType}");
        }

        dynamic dynHandler = handler;
        var response = await dynHandler.HandleAsync((dynamic)request, cancellationToken);

        logger.LogInformation("[QUERY] {Query} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());
        return (TResult)response!;
    }

    public async Task Send(INotification request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("[NOTIFICATION] {Notification} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());

        var handlerInterface = typeof(INotificationHandler<>).MakeGenericType(request.GetType());

        // First try single handler (common registration pattern in tests and simple DI setups)
        var singleHandler = provider.GetService(handlerInterface);
        if (singleHandler != null)
        {
            dynamic dynHandler = singleHandler;
            await dynHandler.HandleAsync((dynamic)request, cancellationToken);
            logger.LogInformation("[NOTIFICATION] {Notification} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());
            return;
        }

        // Fallback: try IEnumerable<THandler> to support multiple handlers
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerInterface);
        var handlersObj = provider.GetService(enumerableType) as IEnumerable<object>;

        if (handlersObj != null && handlersObj.Any())
        {
            foreach (dynamic handler in handlersObj)
            {
                await handler.HandleAsync((dynamic)request, cancellationToken);
            }

            logger.LogInformation("[NOTIFICATION] {Notification} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());
            return;
        }

        logger.LogWarning("No notification handlers registered for {NotificationType}", request.GetType());
        throw new InvalidOperationException($"No handler registered for {handlerInterface}");
    }

    public async Task<TResult> Send<TResult>(INotification<TResult> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("[NOTIFICATION] {Notification} with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());

        var handlerType = typeof(INotificationHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        var handler = provider.GetService(handlerType);
        if (handler == null)
        {
            logger.LogWarning("No notification handler registered for {NotificationType}", request.GetType());
            throw new InvalidOperationException($"No handler registered for {handlerType}");
        }

        dynamic dynHandler = handler;
        var response = await dynHandler.HandleAsync((dynamic)request, cancellationToken);

        logger.LogInformation("[NOTIFICATION] {Notification} handled with CorrelationId: {CorrelationId}", request.GetType().Name, request.CorrelationId());
        return (TResult)response!;
    }

    public async Task<TResult?> Send<TResult>(object request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var correlationId = Guid.NewGuid();
        logger.LogInformation("[SEND OBJECT GENERIC] {Request} with CorrelationId: {CorrelationId}", request.GetType().Name, correlationId);

        var handlerInterfacesToTry = new[] { typeof(ICommandHandler<,>), typeof(IQueryHandler<,>), typeof(INotificationHandler<,>) };

        foreach (var openHandler in handlerInterfacesToTry)
        {
            var specificHandlerType = openHandler.MakeGenericType(request.GetType(), typeof(TResult));
            var handler = provider.GetService(specificHandlerType);
            if (handler != null)
            {
                dynamic dynHandler = handler;
                var response = await dynHandler.HandleAsync((dynamic)request, cancellationToken);
                logger.LogInformation("[SEND OBJECT GENERIC] {Request} with CorrelationId: {CorrelationId}", request.GetType().Name, correlationId);
                return (TResult?)response;
            }
        }

        logger.LogWarning("No handler found for request type {RequestType} -> {ResultType}", request.GetType(), typeof(TResult));
        return default;
    }
}
