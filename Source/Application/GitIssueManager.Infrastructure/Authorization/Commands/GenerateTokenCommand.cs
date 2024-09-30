using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace GitIssueManager.Infrastructure.Authorization.Commands
{
    public class GenerateTokenCommand(AuthenticateResult authenticateResult) : IRequest<string>
    {
        public AuthenticateResult AuthenticateResult => authenticateResult;
    }
}
