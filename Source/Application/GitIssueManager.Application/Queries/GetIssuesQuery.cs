using GitIssueManager.Contract.ReadModels;
using MediatR;

namespace GitIssueManager.Application.Queries;

public class GetIssuesQuery : IRequest<IEnumerable<IssueReadModel>>
{

}
