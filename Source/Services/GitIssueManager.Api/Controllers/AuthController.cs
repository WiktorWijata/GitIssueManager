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
            var provider = ProviderTypes.GitHub.ToString();

            if (!string.IsNullOrEmpty(redirectUri))
            {
                return Challenge(new AuthenticationProperties { RedirectUri = $"/auth/callback/?redirectUri={redirectUri}" }, provider);
            }

            return Challenge(new AuthenticationProperties { RedirectUri = $"/auth/callback" }, provider);
        }

        [HttpGet("login-with-gitlab")]
        public async Task<IActionResult> LoginWithGitLab(string? redirectUri)
        {
            var provider = ProviderTypes.GitLab.ToString();

            if (!string.IsNullOrEmpty(redirectUri))
            {
                return Challenge(new AuthenticationProperties { RedirectUri = $"/auth/callback/?redirectUri={redirectUri}" }, provider);
            }

            return Challenge(new AuthenticationProperties { RedirectUri = $"/auth/callback" }, provider);
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
