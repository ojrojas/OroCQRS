// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;

/// <summary>
/// Represents the base interface for a message with a correlation identifier.
/// </summary>
public interface IBaseMessage
{
    /// Gets the unique identifier used to correlate this message with related messages or processes.
    /// </summary>
    /// <returns>A <see cref="Guid"/> representing the correlation ID.</returns>
    public Guid CorrelationId();
}