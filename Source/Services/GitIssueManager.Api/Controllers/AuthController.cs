using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using GitIssueManager.Infrastructure;
using GitIssueManager.Infrastructure.Authorization.Commands;
using MediatR;

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
            return await this.Challenge(ProviderTypes.GitHub.ToString(), redirectUri);
        }

        [HttpGet("login-with-gitlab")]
        public async Task<IActionResult> LoginWithGitLab(string? redirectUri)
        {
            return await this.Challenge(ProviderTypes.GitLab.ToString(), redirectUri);
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

        private async Task<IActionResult> Challenge(string provider, string redirectUri)
        {
            if (!string.IsNullOrEmpty(redirectUri))
            {
                return base.Challenge(new AuthenticationProperties { RedirectUri = $"/auth/callback/?redirectUri={redirectUri}" }, provider);
            }

            return base.Challenge(new AuthenticationProperties { RedirectUri = $"/auth/callback" }, provider);
        }
    }
}
