using GitIssueManager.Contract.ReadModels;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate;

public class CreateIssueCommand(long repoId, string repo, string title, string body) : IRequest<IssueReadModel>
{
    public long RepoId => repoId;
    public string Repo => repo;
    public string Title => title;
    public string Body => body;
}
