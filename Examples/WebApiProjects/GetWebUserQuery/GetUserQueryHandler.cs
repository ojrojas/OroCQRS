using OroCQRS.Core.Interfaces;

namespace GetWebUserQuery;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, string>
{
    public Task<string> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        // Simulate fetching user data
        return Task.FromResult($"User {query.UserId}: John Doe");
    }
}