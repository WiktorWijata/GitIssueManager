using System.Collections.Generic;

namespace GitIssueManager.Contract.ReadModels
{
    public class RepoReadModel
    {
        public string Name { get; set; }
        public IEnumerable<IssueReadModel> Issues { get; set; }
    }
}
