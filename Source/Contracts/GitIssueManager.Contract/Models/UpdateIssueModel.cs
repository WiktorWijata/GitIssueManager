﻿namespace GitIssueManager.Contract.Models
{
    public class UpdateIssueModel
    {
        public string Repo { get; set; }
        public long IssueNumber { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}