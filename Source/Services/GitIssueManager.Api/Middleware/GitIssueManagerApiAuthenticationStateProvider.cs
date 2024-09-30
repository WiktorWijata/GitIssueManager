using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GitIssueManager.Api.Middleware
{
    public class GitIssueManagerApiAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = httpContextAccessor.HttpContext.User;
            return new AuthenticationState(user);
        }
    }
}
