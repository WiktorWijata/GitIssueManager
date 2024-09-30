using System.Threading.Tasks;
using GitIssueManager.ExternalApi.Contracts.GitHubApi.Models;
using GitIssueManager.ExternalApi.Contracts.GitHubApi.Response;
using Refit;

namespace GitIssueManager.ExternalApi.Contracts.GitHubApi
{
    public interface IGitHubApi
    {
        [Get("/repos/{owner}/{repo}/issues?state=all")]
        Task<IssueResponse[]> GetIssuesForRepo(string owner, string repo);

        [Post("/repos/{owner}/{repo}/issues")]
        Task<IssueResponse> CreateIssue(string owner, string repo, [Body] IssueModel model);

        [Patch("/repos/{owner}/{repo}/issues/{issue_number}")]
        Task<IssueResponse> UpdateIssue(string owner, string repo, long issue_number, [Body] IssueModel model);

        [Get("/users/{username}/repos")]
        Task<RepoResponse[]> GetRepos(string username);
    }
}
