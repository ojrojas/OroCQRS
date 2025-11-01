// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Services;

public class Sender(
    ILogger<Sender> logger,
    IServiceProvider provider
    ) : ISender
{
    public async Task Send(ICommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[COMMAND] {request.GetType().Name} with CorrelationId: {request.CorrelationId()}");
        var handlerTypeCommand = typeof(ICommandHandler<>).MakeGenericType(request.GetType());
        dynamic handler = provider.GetRequiredService(handlerTypeCommand);
        await handler.HandleAsync((dynamic)request, cancellationToken);
        logger.LogInformation($"[COMMAND] {request.GetType().Name} with CorrelationId: {request.CorrelationId}");
    }

    public async Task<TResult> Send<TResult>(ICommand<TResult> request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[COMMAND] {request.GetType().Name} with CorrelationId: {request.CorrelationId()}");
        var handlerTypeCommand = typeof(ICommandHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        dynamic handler = provider.GetRequiredService(handlerTypeCommand);
        var response = await handler.HandleAsync((dynamic)request, cancellationToken);
        logger.LogInformation($"[COMMAND] {request.GetType().Name} with CorrelationId: {request.CorrelationId}");
        return response;
    }

    public async Task<TResult> Send<TResult>(IQuery<TResult> request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[QUERY] {request.GetType().Name} with CorrelationId: {request.CorrelationId()}");
        var handlerTypeCommand = typeof(IQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        dynamic handler = provider.GetRequiredService(handlerTypeCommand);
        var response = await handler.HandleAsync((dynamic)request, cancellationToken);
        logger.LogInformation($"[QUERY] {request.GetType().Name} with CorrelationId: {request.CorrelationId}");
        return response;
    }

    public async Task Send(INotification request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[NOTIFICATION] {request.GetType().Name} with CorrelationId: {request.CorrelationId()}");
        var handlerTypeCommand = typeof(INotificationHandler<>).MakeGenericType(request.GetType());
        dynamic handler = provider.GetRequiredService(handlerTypeCommand);
        await handler.HandleAsync((dynamic)request, cancellationToken);
        logger.LogInformation($"[NOTIFICATION] {request.GetType().Name} with CorrelationId: {request.CorrelationId}");
    }

    public async Task<TResult> Send<TResult>(INotification<TResult> request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[NOTIFICATION] {request.GetType().Name} with CorrelationId: {request.CorrelationId()}");
        var handlerTypeCommand = typeof(INotificationHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        dynamic handler = provider.GetRequiredService(handlerTypeCommand);
        var response = await handler.HandleAsync((dynamic)request, cancellationToken);
        logger.LogInformation($"[NOTIFICATION] {request.GetType().Name} with CorrelationId: {request.CorrelationId}");
        return response;
    }

    public async Task<TResult?> Send<TResult>(object request, CancellationToken cancellationToken)
    {
        var CorrelationId = Guid.NewGuid();
         logger.LogInformation($"[SEND OBJECT GENERIC] {request.GetType().Name} with CorrelationId: {CorrelationId}");
        var handlerTypeCommand = typeof(object).MakeGenericType(request.GetType(), typeof(TResult));
        dynamic handler = provider.GetRequiredService(handlerTypeCommand);
        var response = await handler.HandleAsync((dynamic)request, cancellationToken);
        logger.LogInformation($"[SEND OBJECT GENERIC] {request.GetType().Name} with CorrelationId: {CorrelationId}");
        return response;
    }
}