// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;

/// <summary>
/// Represents a notification that can be handled by a mediator.
/// This interface extends <see cref="IRequest"/> to indicate that it is a request
/// with no expected response, typically used for signaling or broadcasting events.
/// </summary>
public interface INotification : IRequest {}

/// Represents a notification that can produce a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result produced by the notification.</typeparam>
public interface INotification<TResult> : INotification
{
}