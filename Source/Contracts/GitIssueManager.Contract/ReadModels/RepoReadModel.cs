using System.Collections.Generic;

namespace GitIssueManager.Contract.ReadModels
{
    public class RepoReadModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<IssueReadModel> Issues { get; set; }
    }
}
