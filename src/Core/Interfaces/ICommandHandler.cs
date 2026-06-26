// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;

/// <summary>
/// Defines a handler for processing commands of a specific type.
/// </summary>
/// <typeparam name="TCommand">The type of the command to be handled. Must implement the <see cref="ICommand"/> interface.</typeparam>
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    /// <summary>
    /// Handles the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to be processed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for processing commands of type <typeparamref name="TCommand"/> and returning a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TCommand">The type of the command to be handled. Must implement <see cref="ICommand{TResult}"/>.</typeparam>
/// <typeparam name="TResult">The type of the result produced by handling the command.</typeparam>
public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Handles the specified command asynchronously and returns a result.
    /// </summary>
    /// <param name="command">The command to be processed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the command handling.</returns>
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}