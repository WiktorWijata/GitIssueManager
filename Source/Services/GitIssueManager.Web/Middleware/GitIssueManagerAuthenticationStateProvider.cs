using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using GitIssueManager.Infrastructure.Authorization;
using GitIssueManager.Web.Queries;
using GitIssueManager.Web.Commands;
using MediatR;

namespace GitIssueManager.Web.Middleware
{
    public class GitIssueManagerAuthenticationStateProvider(IMediator mediator, IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await mediator.Send(new GetTokenFromCookiesQuery());
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(anonymous);
            }

            var user = this.GetClaimsPrincipal(token);
            return new AuthenticationState(user);
        }

        public async Task NotifyUserAuthentication(string token)
        {
            var user = this.GetClaimsPrincipal(token);
            await mediator.Send(new SaveTokenInCookiesCommand(token));
            base.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task NotifyUserLogout()
        {
            await mediator.Send(new RemoveTokenFromCookiesCommand());
            base.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        private ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, AuthorizationConstants.AccessToken);
            return new ClaimsPrincipal(identity);
        }
    }        
}
