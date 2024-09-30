using System.Net.Http.Headers;
using GitIssueManager.Web.Queries;
using MediatR;

namespace GitIssueManager.Web.Middleware
{
    public class GitIssueManagerHttpsClientHandler(IMediator mediator) : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await mediator.Send(new GetTokenFromCookiesQuery());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
