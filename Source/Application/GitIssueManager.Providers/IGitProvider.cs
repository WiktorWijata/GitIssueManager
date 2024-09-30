using GitIssueManager.Contract.ReadModels;

namespace GitIssueManager.Providers;

public interface IGitProvider
{
    public Task<IEnumerable<RepoReadModel>> GetRepos(string userName);
    public Task<IssueReadModel> CreateIssue(long repoId, string repo, string title, string body);
    public Task<IssueReadModel> UpdateIssue(long repoId, string repo, long issueNumber, string title, string body);
    public Task<IssueReadModel> CloseIssue(long repoId, string repo, long issueNumber, string title, string body);
}
