using Microsoft.Extensions.DependencyInjection;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using GitIssueManager.Providers;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate;

public class CreateIssueCommandHandler(IUserIdentity userIdentity, IServiceProvider serviceProvider) 
    : IRequestHandler<CreateIssueCommand, IssueReadModel>
{
    public async Task<IssueReadModel> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        var provider = serviceProvider.GetRequiredKeyedService<IGitProvider>(userIdentity.ProviderType);
        var issue = await provider.CreateIssue(request.RepoId, request.Repo, request.Title, request.Body);
        return issue;
    }
}
