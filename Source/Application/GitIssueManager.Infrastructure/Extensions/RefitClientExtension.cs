using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

namespace GitIssueManager.Infrastructure.Extensions;

public static class RefitClientExtension
{
    public static IServiceCollection AddRefitClient<T>(this IServiceCollection services, string uri, Type httpClientHandler) 
        where T : class
    {
        services.TryAddTransient(httpClientHandler);
        services.AddHttpClient<T>(c => { c.BaseAddress = new Uri(uri); })
            .ConfigurePrimaryHttpMessageHandler(o => (HttpClientHandler)o.GetRequiredService(httpClientHandler))
            .AddTypedClient(c => RestService.For<T>(c));

        return services;
    }
}
