// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;
/// <summary>
/// Defines a handler for processing notifications of a specific type.
/// </summary>
/// <typeparam name="TNotification">The type of notification to handle. Must implement <see cref="INotification"/>.</typeparam>
public interface INotificationHandler<in TNotification> where TNotification : INotification
{
     /// Handles the specified notification asynchronously and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the handler.</typeparam>
    /// <param name="notification">The notification to be handled.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}

/// Defines a handler for processing notifications of a specific type and returning a result.
/// </summary>
/// <typeparam name="TNotification">
/// The type of the notification to be handled. Must implement <see cref="INotification{TResult}"/>.
/// </typeparam>
/// <typeparam name="TResult">
/// The type of the result produced after handling the notification.
/// </typeparam>
public interface INotificationHandler<in TNotification, TResult>
where TNotification : INotification<TResult>
{
    /// Handles the specified notification asynchronously and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the handler.</typeparam>
    /// <param name="notification">The notification to be handled.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the handling.</returns>
    ValueTask<TResult> HandleAsync(
        TNotification notification, CancellationToken cancellationToken);
}