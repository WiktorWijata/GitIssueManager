using System.Text.Json.Serialization;

namespace GitIssueManager.ExternalApi.Contracts.GitLabApi.Models
{
    public class IssueModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [JsonPropertyName("state_event")]
        public string StateEvent { get; set; }
    }
}
