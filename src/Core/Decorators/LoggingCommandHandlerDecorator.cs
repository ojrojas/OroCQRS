// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Decorators;

public class LoggingCommandHandlerDecorator<TCommand, TResult>(
    ILogger<LoggingCommandHandlerDecorator<TCommand, TResult>> logger,
    ICommandHandler<TCommand, TResult> innerHandler
)
: ICommandHandler<TCommand, TResult>

where TCommand : ICommand<TResult>
{
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[COMMAND] {typeof(TCommand).Name} with CorrelationId: {command.CorrelationId}");
        var result = await innerHandler.HandleAsync(command, cancellationToken);
        logger.LogInformation($"[COMMAND] {typeof(TCommand).Name} with CorrelationId: {command.CorrelationId}");
        return result;
    }
}

public class LoggingCommandHandlerDecorator<TCommand>(
   ILogger<LoggingCommandHandlerDecorator<TCommand>> logger,
   ICommandHandler<TCommand> innerHandler
) : ICommandHandler<TCommand>

where TCommand : ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[COMMAND] {typeof(TCommand).Name} with CorrelationId: {command.CorrelationId}");
        await innerHandler.HandleAsync(command, cancellationToken);
        logger.LogInformation($"[COMMAND] {typeof(TCommand).Name} with CorrelationId: {command.CorrelationId}");
    }
}