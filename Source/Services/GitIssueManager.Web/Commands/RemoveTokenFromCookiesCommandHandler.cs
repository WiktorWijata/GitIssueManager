using GitIssueManager.Infrastructure.Authorization;
using MediatR;

namespace GitIssueManager.Web.Commands
{
    public class RemoveTokenFromCookiesCommandHandler(IHttpContextAccessor httpContextAccessor) : IRequestHandler<RemoveTokenFromCookiesCommand>
    {
        public async Task Handle(RemoveTokenFromCookiesCommand request, CancellationToken cancellationToken)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(AuthorizationConstants.AccessToken);
        }
    }
}
