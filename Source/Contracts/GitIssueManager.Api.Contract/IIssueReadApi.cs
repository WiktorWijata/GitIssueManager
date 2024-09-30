using GitIssueManager.Contract.ReadModels;
using Refit;

namespace GitIssueManager.Api.Contract
{
    public interface IIssueReadApi
    {
        [Get("/issue-read/get-all")]
        Task<IssueReadModel[]> GetIssues();

        [Get("/issue-read/{owner}/{repo}/get/{issueNumber}")]
        Task<IssueReadModel> GetIssue(string owner, string repo, long issueNumber);

        [Get("/issue-read/get-repos-by-user-name/{userName}")]
        Task<RepoReadModel[]> GetRepos(string userName);
    }
}
