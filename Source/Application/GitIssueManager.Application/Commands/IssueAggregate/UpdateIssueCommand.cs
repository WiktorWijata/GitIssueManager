using GitIssueManager.Contract.ReadModels;
using MediatR;

namespace GitIssueManager.Application.Commands.IssueAggregate;

public class UpdateIssueCommand(string repo, long issueNumber, string title, string body) : IRequest<IssueReadModel>
{
    public string Repo => repo;
    public long IssueNumber => issueNumber;
    public string Title => title;
    public string Body => body;
}
