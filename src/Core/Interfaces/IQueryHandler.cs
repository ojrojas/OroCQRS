// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;

/// <summary>
/// Defines a handler for processing queries of type <typeparamref name="TQuery"/> and returning a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TQuery">The type of the query to be handled. Must implement <see cref="IQuery{TResult}"/>.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the query handler.</typeparam>
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    /// <summary>
    /// Handles the specified query asynchronously and returns the result.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query to handle.</typeparam>
    /// <typeparam name="TResult">The type of the result to return.</typeparam>
    /// <param name="query">The query to process.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the query.</returns>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

