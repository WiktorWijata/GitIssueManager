using GitIssueManager.Contract.Models;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.Application.Commands.IssueAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;

namespace GitIssueManager.Api.Controllers;

[ApiController]
[Route("issue")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class IssueController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost("create")]
    [Produces(typeof(IssueReadModel))]
    public async Task<IActionResult> Create(CreateIssueModel createIssueModel)
    {
        var command = mapper.Map<CreateIssueCommand>(createIssueModel);
        var createdIssue = await mediator.Send(command);
        return Ok(createdIssue);
    }

    [HttpPatch("update")]
    [Produces(typeof(IssueReadModel))]
    public async Task<IActionResult> Update(UpdateIssueModel updateIssueModel)
    {
        var command = mapper.Map<UpdateIssueCommand>(updateIssueModel);
        var updatedIssue = await mediator.Send(command);
        return Ok(updatedIssue);
    }

    [HttpPatch("close")]
    [Produces(typeof(IssueReadModel))]
    public async Task<IActionResult> Close(CloseIssueModel closeIssueModel)
    {
        var command = mapper.Map<CloseIssueCommand>(closeIssueModel);
        var updatedIssue = await mediator.Send(command);
        return Ok(updatedIssue);
    }
}
