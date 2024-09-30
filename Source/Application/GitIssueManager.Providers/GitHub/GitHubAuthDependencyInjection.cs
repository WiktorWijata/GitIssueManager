using System.Text.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using GitIssueManager.Providers;

namespace GitIssueManager.Infrastructure.Authorization.GitHub
{
    public static class GitHubAuthDependencyInjection
    {
        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder authenticationBuilder, string clientId, string clientSecret)
        {
            authenticationBuilder.AddOAuth(ProviderTypes.GitHub.ToString(), options =>
            {
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.CallbackPath = new PathString("/signin-github");

                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.UserInformationEndpoint = "https://api.github.com/user";
                options.SaveTokens = true;
                options.Scope.Add("repo");
                options.Scope.Add("read:org");
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, GitHubClaims.Id);
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
                options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, GitHubClaims.Name);
                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();
                        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                        context.RunClaimActions(json.RootElement);
                        context.Identity.AddClaim(new Claim(AuthorizationConstants.ProviderAccessToken, context.AccessToken));
                        context.Identity.AddClaim(new Claim(AuthorizationConstants.ProviderKey, ProviderTypes.GitHub.ToString()));
                    }
                };
            });

            return authenticationBuilder;
        }
    }
}
