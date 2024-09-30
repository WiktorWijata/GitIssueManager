using AutoMapper;
using GitIssueManager.Application.Commands.IssueAggregate;
using GitIssueManager.Contract.Models;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.ExternalApi.Contracts.GitHubApi.Response;

namespace GitIssueManager.Api.Mapping
{
    public class IssueProfile : Profile
    {
        public IssueProfile()
        {
            this.CreateMap<CreateIssueModel, CreateIssueCommand>();
            this.CreateMap<UpdateIssueModel, UpdateIssueCommand>();
            this.CreateMap<IssueResponse, IssueReadModel>();
            this.CreateMap<RepoResponse, RepoReadModel>();
            this.CreateMap<CloseIssueModel, CloseIssueCommand>();
        }
    }
}
