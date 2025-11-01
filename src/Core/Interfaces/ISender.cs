// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;

public interface ISender
{
    Task Send(ICommand request, CancellationToken cancellationToken);
    Task<TResult> Send<TResult>(ICommand<TResult> request, CancellationToken cancellationToken);

    Task Send(INotification request, CancellationToken cancellationToken);
    Task<TResult> Send<TResult>(INotification<TResult> request, CancellationToken cancellationToken);

    Task<TResult> Send<TResult>(IQuery<TResult> request, CancellationToken cancellationToken);
    Task<TResult?> Send<TResult>(object request, CancellationToken cancellationToken);
}