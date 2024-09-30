namespace GitIssueManager.Contract.Models
{
    public class CreateIssueModel
    {
        public long RepoId { get; set; }
        public string Repo { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
