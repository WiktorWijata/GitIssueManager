using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GitIssueManager.Api.Middleware
{
    public class GitIssueManagerApiAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = httpContextAccessor.HttpContext.User;
            return new AuthenticationState(user);
        }
    }
}
