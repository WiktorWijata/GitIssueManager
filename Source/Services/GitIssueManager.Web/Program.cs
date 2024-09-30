using Microsoft.AspNetCore.Authentication.Cookies;
using GitIssueManager.Api.Contract;
using GitIssueManager.Web;
using GitIssueManager.Web.Commands;
using GitIssueManager.Web.Components;
using GitIssueManager.Web.Middleware;
using Refit;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using Blazored.Modal;

var builder = WebApplication.CreateBuilder(args);
var externalRoutes = builder.Configuration.GetSection("ExternalRoutes");
builder.Services.Configure<ExternalRoutes>(builder.Configuration.GetSection("ExternalRoutes"));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(SaveTokenInCookiesCommandHandler).Assembly));
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazoredModal();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<GitIssueManagerAuthenticationStateProvider>();
builder.Services.AddScoped<IUserIdentity, UserIdentity<GitIssueManagerAuthenticationStateProvider>>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddTransient<GitIssueManagerHttpsClientHandler>();
builder.Services.AddHttpClient<IIssueApi>(c => { c.BaseAddress = new Uri(externalRoutes["IssueApi"]); })
    .ConfigurePrimaryHttpMessageHandler<GitIssueManagerHttpsClientHandler>()
    .AddTypedClient(c => RestService.For<IIssueApi>(c));

builder.Services.AddHttpClient<IIssueReadApi>(c => { c.BaseAddress = new Uri(externalRoutes["IssueApi"]); })
    .ConfigurePrimaryHttpMessageHandler<GitIssueManagerHttpsClientHandler>()
    .AddTypedClient(c => RestService.For<IIssueReadApi>(c));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();

app.Run();