// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;


/// <summary>
/// Represents a command in the CQRS (Command Query Responsibility Segregation) pattern.
/// Commands are used to encapsulate a request to perform an action or change the state of the system.
/// </summary>
public interface ICommand : IRequest
{
}

/// Represents a command that returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the command.</typeparam>
public interface ICommand<out TResult> : IRequest<TResult> { }
