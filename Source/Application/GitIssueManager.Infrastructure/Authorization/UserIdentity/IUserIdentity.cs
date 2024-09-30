namespace GitIssueManager.Infrastructure.Authorization.UserIdentity
{
    public interface IUserIdentity
    {
        public string Id { get; }
        public string Name { get; }
        public string Provider { get; }
        public string UserName { get; }
        public bool IsAuthenticated { get; }
        public ProviderTypes ProviderType { get; }
    }
}
