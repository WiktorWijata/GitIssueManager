using MediatR;

namespace GitIssueManager.Web.Queries
{
    public class GetTokenFromCookiesQuery() : IRequest<string>
    {
    }
}
