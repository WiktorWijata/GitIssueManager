using GitIssueManager.Infrastructure.Authorization;
using MediatR;

namespace GitIssueManager.Web.Queries
{
    public class GetTokenFromCookiesQueryHandler(IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetTokenFromCookiesQuery, string>
    {
        public async Task<string> Handle(GetTokenFromCookiesQuery request, CancellationToken cancellationToken)
        {
            httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(AuthorizationConstants.AccessToken, out var token);
            return token;
        }
    }
}
