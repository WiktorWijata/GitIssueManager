using GitIssueManager.Contract.ReadModels;
using MediatR;

namespace GitIssueManager.Application.Queries;

public class GetIssueQuery(string owner, string repo, long issueNumber) : IRequest<IssueReadModel>
{
    public string Owner => owner;
    public string Repo => repo;
    public long IssueNumber => issueNumber;
}
