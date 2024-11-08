using Microsoft.Extensions.DependencyInjection;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using GitIssueManager.Providers;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate
{
    public class UpdateIssueCommandHandler(IUserIdentity userIdentity, IServiceProvider serviceProvider) 
        : IRequestHandler<UpdateIssueCommand, IssueReadModel>
    {
        public async Task<IssueReadModel> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
        {
            var provider = serviceProvider.GetRequiredKeyedService<IGitProvider>(userIdentity.ProviderType);
            var updatedIssue = await provider.UpdateIssue(request.RepoId, request.Repo, request.IssueNumber, request.Title, request.Body);
            return updatedIssue;
        }
    }
}
