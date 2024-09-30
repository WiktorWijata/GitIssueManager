using GitIssueManager.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace GitIssueManager.Providers.GitLab
{
    public class GitLabHttpsClientHandler(IHttpContextAccessor httpContextAccessor) : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var user = httpContextAccessor.HttpContext.User;
            var token = user.Claims.Single(x => x.Type == AuthorizationConstants.ProviderAccessToken).Value;
            var decryptedToken = TokenEncryption.Decrypt(token);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", decryptedToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.UserAgent.ParseAdd("xOpero.GitIssueManager.Api");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
