using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using GitIssueManager.Infrastructure.Authorization;

namespace GitIssueManager.Api.Middleware;

public class GitHubHttpsClientHandler(IHttpContextAccessor httpContextAccessor) : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var user = httpContextAccessor.HttpContext.User;
        var token = user.Claims.Single(x => x.Type == AuthorizationConstants.ProviderAccessToken).Value;
        var decryptedToken = TokenEncryption.Decrypt(token);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", decryptedToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        request.Headers.UserAgent.ParseAdd("xOpero.GitIssueManager.Api");
        request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");
        return await base.SendAsync(request, cancellationToken);
    }
}
