// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Decorators;

public class LoggingQueryHandlerDecorator<TQuery, TResult>
(ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger,
IQueryHandler<TQuery, TResult> innerHandler)
: IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[QUERY] {typeof(TQuery).Name} with CorrelationId: {query.CorrelationId}");
        var result = await innerHandler.HandleAsync(query, cancellationToken);
        logger.LogInformation($"[QUERY] {typeof(TQuery).Name} with CorrelationId: {query.CorrelationId}");
        return result;
    }
}