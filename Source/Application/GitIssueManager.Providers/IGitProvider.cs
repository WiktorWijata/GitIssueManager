using GitIssueManager.Contract.ReadModels;

namespace GitIssueManager.Providers;

public interface IGitProvider
{
    public Task<IEnumerable<RepoReadModel>> GetRepos(string userName);
    public Task<IssueReadModel> CreateIssue(string repo, string title, string body);
    public Task<IssueReadModel> UpdateIssue(string repo, long number, string title, string body);
    public Task<IssueReadModel> CloseIssue(string repo, long number, string title, string body);
}
