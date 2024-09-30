using GitIssueManager.Contract.ReadModels;
using MediatR;

namespace GitIssueManager.Application.Queries
{
    public class GetReposQuery(string userName) : IRequest<IEnumerable<RepoReadModel>>
    {
        public string UserName => userName;
    }
}
