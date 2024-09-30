using GitIssueManager.Infrastructure;
using GitIssueManager.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace GitIssueManager.Providers.GitLab
{
    public static class GitLabAuthDependencyInjection
    {
        public static AuthenticationBuilder AddGitLab(this AuthenticationBuilder authenticationBuilder, string clientId, string clientSecret)
        {
            authenticationBuilder.AddOAuth(ProviderTypes.GitLab.ToString(), options =>
            {
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.CallbackPath = new PathString("/signin-gitlab");

                options.AuthorizationEndpoint = "https://gitlab.com/oauth/authorize";
                options.TokenEndpoint = "https://gitlab.com/oauth/token";
                options.UserInformationEndpoint = "https://gitlab.com/api/v4/user";
                options.SaveTokens = true;
                options.Scope.Add("read_api");
                options.Scope.Add("api");
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, GitLabClaims.Id);
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
                options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, GitLabClaims.Name);
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
                        context.Identity.AddClaim(new Claim(AuthorizationConstants.ProviderKey, ProviderTypes.GitLab.ToString()));
                    }
                };
            });

            return authenticationBuilder;
        }
    }
}
