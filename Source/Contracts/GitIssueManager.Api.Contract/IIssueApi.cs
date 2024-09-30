using GitIssueManager.Contract.Models;
using GitIssueManager.Contract.ReadModels;
using Refit;

namespace GitIssueManager.Api.Contract
{
    public interface IIssueApi
    {
        [Post("/issue/create")]
        Task<IssueReadModel> Create(CreateIssueModel createIssueModel);

        [Patch("/issue/update")]
        Task<IssueReadModel> Update(UpdateIssueModel updateIssueModel);

        [Patch("/issue/close")]
        Task<IssueReadModel> Close(CloseIssueModel closeIssueModel);
    }
}
