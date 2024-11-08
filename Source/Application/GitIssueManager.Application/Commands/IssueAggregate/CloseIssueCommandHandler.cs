using Microsoft.Extensions.DependencyInjection;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using GitIssueManager.Providers;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate;

public class CloseIssueCommandHandler(IUserIdentity userIdentity, IServiceProvider serviceProvider)
    : IRequestHandler<CloseIssueCommand, IssueReadModel>
{
    public async Task<IssueReadModel> Handle(CloseIssueCommand request, CancellationToken cancellationToken)
    {
        var provider = serviceProvider.GetRequiredKeyedService<IGitProvider>(userIdentity.ProviderType);
        var closedIssue = await provider.CloseIssue(request.RepoId, request.Repo, request.IssueNumber, request.Title, request.Body);
        return closedIssue;
    }
}
