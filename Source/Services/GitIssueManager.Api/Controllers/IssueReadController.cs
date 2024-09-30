using GitIssueManager.Application.Queries;
using GitIssueManager.Contract.ReadModels;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace GitIssueManager.Api.Controllers;

[ApiController]
[Route("issue-read")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class IssueReadController(IMediator mediator) : ControllerBase
{
    [HttpGet("get-all")]
    [Produces<IssueReadModel[]>]
    public async Task<IActionResult> GetIssues()
    {
        var issues = await mediator.Send(new GetIssuesQuery());
        return Ok(issues);
    }

    [HttpGet("{owner}/{repo}/get/{issueNumber}")]
    [Produces<IssueReadModel>]
    public async Task<IActionResult> GetIssue(string owner, string repo, long issueNumber)
    {
        var issues = await mediator.Send(new GetIssueQuery(owner, repo, issueNumber));
        return Ok(issues);
    }

    [HttpGet("get-repos-by-user-name/{userName}")]
    [Produces<RepoReadModel[]>]
    public async Task<IActionResult> GetRepos(string userName)
    {
        var issues = await mediator.Send(new GetReposQuery(userName));
        return Ok(issues);
    }
}