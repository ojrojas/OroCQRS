// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;

/// <summary>
/// Provides methods to send commands, queries, and notifications to their registered handlers.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Sends a command with no return value to its registered handler.
    /// </summary>
    Task Send(ICommand request, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a command that returns a result of type <typeparamref name="TResult"/>.
    /// </summary>
    Task<TResult> Send<TResult>(ICommand<TResult> request, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a notification with no return value to its registered handlers.
    /// </summary>
    Task Send(INotification request, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a notification that returns a result of type <typeparamref name="TResult"/>.
    /// </summary>
    Task<TResult> Send<TResult>(INotification<TResult> request, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a query that returns a result of type <typeparamref name="TResult"/>.
    /// </summary>
    Task<TResult> Send<TResult>(IQuery<TResult> request, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a request object with no compile-time type information and attempts to resolve a handler
    /// for it. Tries command, query, and notification handlers in order.
    /// </summary>
    Task<TResult?> Send<TResult>(object request, CancellationToken cancellationToken);
}
