using GitIssueManager.Contract.ReadModels;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate
{
    public class CloseIssueCommand(long repoId, string repo, long issueNumber, string title, string body) : IRequest<IssueReadModel>
    {
        public long RepoId => repoId;
        public string Repo => repo;
        public long IssueNumber => issueNumber;
        public string Title => title;
        public string Body => body;
    }
}
