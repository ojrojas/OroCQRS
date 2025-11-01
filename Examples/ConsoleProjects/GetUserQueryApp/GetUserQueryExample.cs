// OroCQRS
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
using System;
using System.Threading;
using System.Threading.Tasks;
using OroCQRS.Core.Interfaces;

public record GetUserQuery(int UserId) : IQuery<User>
{
    public Guid CorrelationId() => Guid.NewGuid();
}

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, User>
{
    public async Task<User> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Fetching user with ID: {query.UserId}");
        await Task.Delay(100, cancellationToken); // Simulate some work
        return new User { Id = query.UserId, Name = "John Doe" };
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }

    public User()
    {
        Name = string.Empty;
    }
}