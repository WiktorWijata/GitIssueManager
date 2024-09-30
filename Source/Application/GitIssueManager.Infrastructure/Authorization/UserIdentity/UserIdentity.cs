using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GitIssueManager.Infrastructure.Authorization.UserIdentity
{
    public class UserIdentity<T> : IUserIdentity
        where T : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal user;

        public string Id => this.GetClaimValue(JwtRegisteredClaimNames.Sub);
        public string Name => this.GetClaimValue(JwtRegisteredClaimNames.Name);
        public string Provider => this.GetClaimValue(AuthorizationConstants.ProviderKey);
        public string UserName => this.GetClaimValue(AuthorizationConstants.UserName);
        public ProviderTypes ProviderType => (ProviderTypes)Enum.Parse(typeof(ProviderTypes), this.GetClaimValue(AuthorizationConstants.ProviderKey));
        public bool IsAuthenticated => this.user.Identity.IsAuthenticated;

        public UserIdentity(T authenticationStateProvider)
        {
            this.user = authenticationStateProvider.GetAuthenticationStateAsync().GetAwaiter().GetResult().User;
        }

        private string GetClaimValue(string claimType)
        {
            return this.user.FindFirst(claimType)?.Value;
        }
    }
}
