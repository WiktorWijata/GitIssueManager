using Newtonsoft.Json;

namespace GitIssueManager.ExternalApi.Contracts.GitHubApi.Models
{
    public class IssueModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string State { get; set; }
    }
}
