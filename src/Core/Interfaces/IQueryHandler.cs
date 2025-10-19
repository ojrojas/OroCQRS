// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    ValueTask<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

