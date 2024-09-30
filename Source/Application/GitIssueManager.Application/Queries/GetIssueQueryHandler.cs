using AutoMapper;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.ExternalApi.Contracts.GitHubApi;
using MediatR;

namespace GitIssueManager.Application.Queries;

public class GetIssueQueryHandler(IGitHubApi gitHubApi, IMapper mapper) : IRequestHandler<GetIssueQuery, IssueReadModel>
{
    public async Task<IssueReadModel> Handle(GetIssueQuery request, CancellationToken cancellationToken)
    {
        var result = await gitHubApi.GetIssue(request.Owner, request.Repo, request.IssueNumber);
        var issue = mapper.Map<IssueReadModel>(result);
        return issue;
    }
}
