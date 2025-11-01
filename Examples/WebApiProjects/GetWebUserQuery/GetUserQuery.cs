using OroCQRS.Core.Interfaces;

namespace GetWebUserQuery;

public record GetUserQuery(Guid UserId) : IQuery<string>
{
    public Guid CorrelationId() => Guid.NewGuid();
}