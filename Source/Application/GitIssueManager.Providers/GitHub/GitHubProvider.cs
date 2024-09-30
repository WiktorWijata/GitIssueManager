using AutoMapper;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.ExternalApi.Contracts.GitHubApi;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;

namespace GitIssueManager.Providers.GitHub;

public class GitHubProvider(IGitHubApi githubApi, IMapper mapper, IUserIdentity userIdentity) : IGitProvider
{
    public async Task<IEnumerable<RepoReadModel>> GetRepos(string userName)
    {
        var reposResult = await githubApi.GetRepos(userName);
        var repos = mapper.Map<IEnumerable<RepoReadModel>>(reposResult);

        foreach (var repo in repos)
        {
            var issuesResult = await githubApi.GetIssuesForRepo(userName, repo.Name);
            if (issuesResult.Length > 0)
            {
                repo.Issues = mapper.Map<IEnumerable<IssueReadModel>>(issuesResult);
            }
        }

        return repos;
    }

    public async Task<IssueReadModel> CreateIssue(long repoId, string repo, string title, string body)
    {
        var issue = await githubApi.CreateIssue(
            userIdentity.UserName, 
            repo, 
            new ExternalApi.Contracts.GitHubApi.Models.IssueModel() 
            { 
                Title = title, 
                Body = body 
            });

        return mapper.Map<IssueReadModel>(issue);
    }

    public async Task<IssueReadModel> UpdateIssue(long repoId, string repo, long issueNumber, string title, string body)
    {
        var issue = await githubApi.UpdateIssue(
            userIdentity.UserName, 
            repo,
            issueNumber,
            new ExternalApi.Contracts.GitHubApi.Models.IssueModel()
            {
                Title = title,
                Body = body,
                State = "open"
            });

        return mapper.Map<IssueReadModel>(issue);
    }

    public async Task<IssueReadModel> CloseIssue(long repoId, string repo, long issueNumber, string title, string body)
    {
        var issue = await githubApi.UpdateIssue(
            userIdentity.UserName,                
            repo,
            issueNumber,
            new ExternalApi.Contracts.GitHubApi.Models.IssueModel()
            {
                Title = title,
                Body = body,
                State = "closed"
            });

        return mapper.Map<IssueReadModel>(issue);
    }
}
