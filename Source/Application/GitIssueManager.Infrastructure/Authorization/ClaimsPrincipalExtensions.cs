using System.Security.Claims;

namespace GitIssueManager.Infrastructure.Authorization
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {

            return claimsPrincipal.Claims.Single(x => x.Type == claimType).Value;
        }
    }
}
