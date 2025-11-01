// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;

/// <summary>
/// Represents a query that returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the query.</typeparam>
public interface IQuery<out TResult> : IRequest<TResult>
{
}