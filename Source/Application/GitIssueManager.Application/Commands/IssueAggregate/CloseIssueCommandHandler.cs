using GitIssueManager.Contract.ReadModels;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using GitIssueManager.Infrastructure;
using GitIssueManager.Providers;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate;

public class CloseIssueCommandHandler(IUserIdentity userIdentity, Func<ProviderTypes, IGitProvider> providerFactory)
    : IRequestHandler<CloseIssueCommand, IssueReadModel>
{
    public async Task<IssueReadModel> Handle(CloseIssueCommand request, CancellationToken cancellationToken)
    {
        var provider = providerFactory(userIdentity.ProviderType);
        var closedIssue = await provider.CloseIssue(request.Repo, request.IssueNumber, request.Title, request.Body);
        return closedIssue;
    }
}
