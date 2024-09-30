using MediatR;

namespace GitIssueManager.Web.Commands
{
    public class SaveTokenInCookiesCommand(string token) : IRequest
    { 
        public string Token => token;
    }
}
