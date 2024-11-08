using Microsoft.Extensions.DependencyInjection;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using GitIssueManager.Providers;
using MediatR;

namespace GitIssueManager.Application.Queries;

public class GetReposQueryHandler(IUserIdentity userIdentity, IServiceProvider serviceProvider) 
    : IRequestHandler<GetReposQuery, IEnumerable<RepoReadModel>>
{
    public async Task<IEnumerable<RepoReadModel>> Handle(GetReposQuery request, CancellationToken cancellationToken)
    {
        var provider = serviceProvider.GetRequiredKeyedService<IGitProvider>(userIdentity.ProviderType);
        var repos = await provider.GetRepos(userIdentity.UserName);
        return repos;
    }
}
