namespace GitIssueManager.ExternalApi.Contracts.GitHubApi.Response
{
    public class IssueResponse
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public long Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
