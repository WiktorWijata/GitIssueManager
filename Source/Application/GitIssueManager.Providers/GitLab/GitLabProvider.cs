using AutoMapper;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.ExternalApi.Contracts.GitLabApi;
using GitIssueManager.ExternalApi.Contracts.GitLabApi.Models;

namespace GitIssueManager.Providers.GitLab;

public class GitLabProvider(IGitLabApi gitLabApi, IMapper mapper) : IGitProvider
{
    public async Task<IssueReadModel> CloseIssue(long repoId, string repo, long issueNumber, string title, string body)
    {
        var issue = await gitLabApi.UpdateIssue(
            repoId,
            issueNumber,
            new IssueModel()
            {
                Title = title,
                Description = body,
                StateEvent = "close"
            });

        return mapper.Map<IssueReadModel>(issue);
    }

    public async Task<IssueReadModel> CreateIssue(long repoId, string repo, string title, string body)
    {
        var issue = await gitLabApi.CreateIssue(
            repoId, 
            new IssueModel()
            {
                Title = title,
                Description = body
            });

        return mapper.Map<IssueReadModel>(issue);
    }

    public async Task<IEnumerable<RepoReadModel>> GetRepos(string userName)
    {
        var projectsResult = await gitLabApi.GetProjects();
        var repos = mapper.Map<IEnumerable<RepoReadModel>>(projectsResult);
        
        foreach (var repo in repos)
        {
            var issues = await gitLabApi.GetIssues(repo.Id);
            if (issues.Length > 0)
            {
                repo.Issues = mapper.Map<IEnumerable<IssueReadModel>>(issues);
            }
        }

        return repos;
    }

    public async Task<IssueReadModel> UpdateIssue(long repoId, string repo, long issueId, string title, string body)
    {
        var issue = await gitLabApi.UpdateIssue(
            repoId,
            issueId,
            new IssueModel()
            {
                Title = title,
                Description = body
            }); 

        return mapper.Map<IssueReadModel>(issue);
    }
}
