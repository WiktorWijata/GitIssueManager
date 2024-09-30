using GitIssueManager.Contract.ReadModels;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using GitIssueManager.Infrastructure;
using GitIssueManager.Providers;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate;

public class CreateIssueCommandHandler(IUserIdentity userIdentity, Func<ProviderTypes, IGitProvider> providerFactory) 
    : IRequestHandler<CreateIssueCommand, IssueReadModel>
{
    public async Task<IssueReadModel> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        var provider = providerFactory(userIdentity.ProviderType);
        var issue = await provider.CreateIssue(request.Repo, request.Title, request.Body);
        return issue;
    }
}
