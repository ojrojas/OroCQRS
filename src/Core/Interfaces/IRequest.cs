// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;
/// <summary>
/// Represents a request message in the system, inheriting from <see cref="IBaseMessage"/>.
/// This interface serves as a marker for request messages that can be processed by the system.
/// </summary>
public interface IRequest : IBaseMessage { }

/// <summary>
/// Represents a request message that returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the request.</typeparam>
public interface IRequest<out TResult> : IBaseMessage { }
