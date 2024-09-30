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

    public async Task<IssueReadModel> CreateIssue(string repo, string title, string body)
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

    public async Task<IssueReadModel> UpdateIssue(string repo, long number, string title, string body)
    {
        var issue = await githubApi.UpdateIssue(
            userIdentity.UserName, 
            repo, 
            number, 
            new ExternalApi.Contracts.GitHubApi.Models.IssueModel() 
            { 
                Title = title, 
                Body = body
            });

        return mapper.Map<IssueReadModel>(issue);
    }

    public async Task<IssueReadModel> CloseIssue(string repo, long number, string title, string body)
    {
        var issue = await githubApi.UpdateIssue(
            userIdentity.UserName,                
            repo,
            number,
            new ExternalApi.Contracts.GitHubApi.Models.IssueModel()
            {
                Title = title,
                Body = body,
                State = "closed"
            });

        return mapper.Map<IssueReadModel>(issue);
    }
}
