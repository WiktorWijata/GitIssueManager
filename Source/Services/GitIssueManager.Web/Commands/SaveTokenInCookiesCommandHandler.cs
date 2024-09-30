using GitIssueManager.Infrastructure.Authorization;
using MediatR;

namespace GitIssueManager.Web.Commands
{
    public class SaveTokenInCookiesCommandHandler(IHttpContextAccessor httpContextAccessor) : IRequestHandler<SaveTokenInCookiesCommand>
    {
        public async Task Handle(SaveTokenInCookiesCommand request, CancellationToken cancellationToken)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Append(AuthorizationConstants.AccessToken, request.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        }
    }
}
