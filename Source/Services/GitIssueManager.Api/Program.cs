using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using GitIssueManager.Api.Mapping;
using GitIssueManager.Application.Commands.IssueAggregate;
using GitIssueManager.Infrastructure.Authorization;
using GitIssueManager.Infrastructure.Authorization.Commands;
using GitIssueManager.Infrastructure.Authorization.GitHub;
using GitIssueManager.Api.Middleware;
using GitIssueManager.ExternalApi.Contracts.GitHubApi;
using GitIssueManager.Providers.GitHub;
using GitIssueManager.Providers;
using GitIssueManager.Infrastructure;
using Refit;
using GitIssueManager.Infrastructure.Authorization.UserIdentity;
using GitIssueManager.Providers.GitLab;
using GitIssueManager.ExternalApi.Contracts.GitLabApi;

var builder = WebApplication.CreateBuilder(args);
var externalRoutes = builder.Configuration.GetSection("ExternalRoutes");
var OAuth = builder.Configuration.GetSection("OAuth");
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateIssueCommandHandler).Assembly, typeof(GenerateTokenCommandHandler).Assembly));
builder.Services.AddAutoMapper(typeof(IssueProfile).Assembly);
builder.Services.AddScoped<GitIssueManagerApiAuthenticationStateProvider>();
builder.Services.AddScoped<IUserIdentity, UserIdentity<GitIssueManagerApiAuthenticationStateProvider>>();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GitIssueManager.Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }});
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGitHub(OAuth["GitHub:ClientId"], OAuth["GitHub:ClientSecret"])
.AddGitLab(OAuth["GitLab:ClientId"], OAuth["GitLab:ClientSecret"])
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
    {
        p.WithOrigins("https://localhost:7130/")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<GitHubHttpsClientHandler>();
builder.Services.AddHttpClient<IGitHubApi>(c => { c.BaseAddress = new Uri(externalRoutes["GitHubApi"]); })
    .ConfigurePrimaryHttpMessageHandler<GitHubHttpsClientHandler>()
    .AddTypedClient(c => RestService.For<IGitHubApi>(c));

builder.Services.AddTransient<GitLabHttpsClientHandler>();
builder.Services.AddHttpClient<IGitLabApi>(c => { c.BaseAddress = new Uri(externalRoutes["GitLabApi"]); })
    .ConfigurePrimaryHttpMessageHandler<GitLabHttpsClientHandler>()
    .AddTypedClient(c => RestService.For<IGitLabApi>(c));

builder.Services.AddTransient<GitHubProvider>();
builder.Services.AddTransient<GitLabProvider>();
builder.Services.AddTransient<Func<ProviderTypes, IGitProvider>>(s => key =>
{
    return key switch
    {
        ProviderTypes.GitHub => s.GetRequiredService<GitHubProvider>(),
        ProviderTypes.GitLab => s.GetRequiredService<GitLabProvider>(),
        _ => throw new NotImplementedException()
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GitIssueManager.Api v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseRouting();
app.UseCookiePolicy(); 
app.UseAuthentication(); 
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
