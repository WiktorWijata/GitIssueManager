using GitIssueManager.Contract.ReadModels;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using GitIssueManager.Infrastructure;
using GitIssueManager.Providers;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate
{
    public class UpdateIssueCommandHandler(IUserIdentity userIdentity, Func<ProviderTypes, IGitProvider> providerFactory) : IRequestHandler<UpdateIssueCommand, IssueReadModel>
    {
        public async Task<IssueReadModel> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
        {
            var provider = providerFactory(userIdentity.ProviderType);
            var updatedIssue = await provider.UpdateIssue(request.Repo, request.IssueNumber, request.Title, request.Body);
            return updatedIssue;
        }
    }
}
