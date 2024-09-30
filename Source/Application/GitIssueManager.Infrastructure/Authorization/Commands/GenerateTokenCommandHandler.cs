using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MediatR;

namespace GitIssueManager.Infrastructure.Authorization.Commands;

public class GenerateTokenCommandHandler(IOptions<JwtConfiguration> options) : IRequestHandler<GenerateTokenCommand, string>
{
    public async Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var jwtConfig = options.Value;
        var user = request.AuthenticateResult.Principal;
        var providerToken = user.Claims.Single(x => x.Type == AuthorizationConstants.ProviderAccessToken).Value;
        var encryptedToken = TokenEncryption.Encrypt(providerToken);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Name, user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.GivenName) == null ? user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name).Value : user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.GivenName).Value ),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
            new Claim(AuthorizationConstants.ProviderKey, user.Claims.Single(x => x.Type == AuthorizationConstants.ProviderKey).Value),
            new Claim(AuthorizationConstants.ProviderAccessToken, encryptedToken),
            new Claim(AuthorizationConstants.UserName, user.Claims.Single(x => x.Type == ClaimTypes.Name).Value)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return jwtToken;
    }
}
