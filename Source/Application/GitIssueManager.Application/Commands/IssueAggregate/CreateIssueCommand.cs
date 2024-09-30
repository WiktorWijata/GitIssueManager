using GitIssueManager.Contract.ReadModels;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate;

public class CreateIssueCommand(string repo, string title, string body) : IRequest<IssueReadModel>
{
    public string Repo => repo;
    public string Title => title;
    public string Body => body;
}
