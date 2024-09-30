using AutoMapper;
using GitIssueManager.Application.Commands.IssueAggregate;
using GitIssueManager.Contract.Models;
using GitIssueManager.Contract.ReadModels;
using GitIssueManager.ExternalApi.Contracts.GitHubApi.Response;
using GitIssueManager.ExternalApi.Contracts.GitLabApi.Response;

namespace GitIssueManager.Api.Mapping
{
    public class IssueProfile : Profile
    {
        public IssueProfile()
        {
            this.CreateMap<CreateIssueModel, CreateIssueCommand>();
            this.CreateMap<UpdateIssueModel, UpdateIssueCommand>();
            this.CreateMap<ExternalApi.Contracts.GitHubApi.Response.IssueResponse, IssueReadModel>();
            this.CreateMap<ExternalApi.Contracts.GitLabApi.Response.IssueResponse, IssueReadModel>()
                .ForMember(d => d.Url, m => m.MapFrom(s => s.WebUrl))
                .ForMember(d => d.Body, m => m.MapFrom(s => s.Description))
                .ForMember(d => d.Number, m => m.MapFrom(s => s.Iid));
            this.CreateMap<RepoResponse, RepoReadModel>();
            this.CreateMap<CloseIssueModel, CloseIssueCommand>();
            this.CreateMap<ProjectResponse, RepoReadModel>();
            this.CreateMap<IssueReadModel, ExternalApi.Contracts.GitLabApi.Models.IssueModel>()
                .ForMember(d => d.Description, m => m.MapFrom(s => s.Body));
        }
    }
}
