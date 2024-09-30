using GitIssueManager.Infrastructure;
using GitIssueManager.Infrastructure.Authorization.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace GitIssueManager.Api.Controllers
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("login-with-github")]
        public async Task<IActionResult> LoginWithGitHub(string? redirectUri)
        {
            if (!string.IsNullOrEmpty(redirectUri))
            {
                return Challenge(new AuthenticationProperties { RedirectUri = $"/auth/callback/?redirectUri={redirectUri}" }, ProviderTypes.GitHub.ToString());
            }

            return Challenge(new AuthenticationProperties { RedirectUri = $"/auth/callback" }, ProviderTypes.GitHub.ToString());
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string? redirectUri)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest();

            var jwtToken = await this.mediator.Send(new GenerateTokenCommand(authenticateResult));

            if (!string.IsNullOrEmpty(redirectUri))
            {
                return Redirect($"{redirectUri}/?token={jwtToken}");
            }

            return Ok(jwtToken);
        }
    }
}
