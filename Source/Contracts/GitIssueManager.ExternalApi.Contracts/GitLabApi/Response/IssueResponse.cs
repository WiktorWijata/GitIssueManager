namespace GitIssueManager.ExternalApi.Contracts.GitLabApi.Response
{
    public class IssueResponse
    {
        public long Id { get; set; }
        public long Iid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string WebUrl { get; set; }
    }
}
