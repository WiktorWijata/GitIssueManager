using Microsoft.AspNetCore.Mvc;
using MediatR;
using GitIssueManager.Web.Middleware;

namespace GitIssueManager.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly GitIssueManagerAuthenticationStateProvider stateProvider;

        public AuthController(IMediator mediator, GitIssueManagerAuthenticationStateProvider stateProvider)
        {
            this.mediator = mediator;
            this.stateProvider = stateProvider;
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string token)
        {
            await this.stateProvider.NotifyUserAuthentication(token);
            return Redirect("/issues");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await this.stateProvider.NotifyUserLogout();
            return Redirect("/");
        }
    }
}
