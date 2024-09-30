using GitIssueManager.ExternalApi.Contracts.GitLabApi.Models;
using GitIssueManager.ExternalApi.Contracts.GitLabApi.Response;
using Refit;
using System.Threading.Tasks;

namespace GitIssueManager.ExternalApi.Contracts.GitLabApi
{
    public interface IGitLabApi
    {
        [Get("/projects")]
        Task<ProjectResponse[]> GetProjects([Query] bool membership = true);

        [Get("/projects/{projectId}/issues")]
        Task<IssueResponse[]> GetIssues(long projectId);

        [Post("/projects/{projectId}/issues")]
        Task<IssueResponse> CreateIssue(long projectId, [Body] IssueModel request);

        [Put("/projects/{projectId}/issues/{issueNumber}")]
        Task<IssueResponse> UpdateIssue(long projectId, long issueNumber, [Body] IssueModel request);
    }
}
