using GitIssueManager.Contract.ReadModels;
using GitIssueManager.Infrastructure;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using GitIssueManager.Providers;
using MediatR;

namespace GitIssueManager.Application.Queries;

public class GetReposQueryHandler(IUserIdentity userIdentity, Func<ProviderTypes, IGitProvider> providerFactory) : IRequestHandler<GetReposQuery, IEnumerable<RepoReadModel>>
{
    public async Task<IEnumerable<RepoReadModel>> Handle(GetReposQuery request, CancellationToken cancellationToken)
    {
        var provider = providerFactory(userIdentity.ProviderType);
        var repos = await provider.GetRepos(userIdentity.UserName);
        return repos;
    }
}
