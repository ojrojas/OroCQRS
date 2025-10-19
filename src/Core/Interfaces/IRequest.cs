// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroCQRS.Core.Interfaces;
public interface IRequest : IBaseMessage { }

public interface IRequest<out IResult> : IBaseMessage { }
