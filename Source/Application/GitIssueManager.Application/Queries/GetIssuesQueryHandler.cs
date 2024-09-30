using GitIssueManager.Contract.ReadModels;
using GitIssueManager.ExternalApi.Contracts.GitHubApi;
using MediatR;
using AutoMapper;

namespace GitIssueManager.Application.Queries;

public class GetIssuesQueryHandler(IGitHubApi gitHubApi, IMapper mapper) : IRequestHandler<GetIssuesQuery, IEnumerable<IssueReadModel>>
{
    public async Task<IEnumerable<IssueReadModel>> Handle(GetIssuesQuery request, CancellationToken cancellationToken)
    {
        //var result = await gitHubApi.GetIssues();
        //var issues = mapper.Map<IEnumerable<IssueReadModel>>(result);
        return null;
    }
}
