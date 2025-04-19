using csharp_RBAC.Configurations;
using csharp_RBAC.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace csharp_RBAC.Services
{
    public class JwtService : IJwtService
    {
        private readonly AppSetting _setting;

        public JwtService(IOptions<AppSetting> setting)
        {
            _setting = setting.Value;
        }

        public string GenerateAccessToken(JwtRequestModel requestModel)
        {
            var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, _setting.Jwt.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(
                JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64
            ),
            new Claim(JwtRegisteredClaimNames.Email, requestModel.Email),
            new Claim("Role", requestModel.Role),
        };

            foreach (var scope in requestModel.Scopes)
            {
                claims.Add(new Claim("scope", scope));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.Jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _setting.Jwt.Issuer,
                _setting.Jwt.Audience,
                claims,
                expires: DateTime.Now.AddHours(_setting.Jwt.AccessTokenExpireInHours),
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(JwtRequestModel requestModel)
        {
            var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, _setting.Jwt.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(
                JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64
            ),
            new Claim(JwtRegisteredClaimNames.Email, requestModel.Email),
            new Claim(ClaimTypes.Role, requestModel.Role),
        };

            foreach (var scope in requestModel.Scopes)
            {
                claims.Add(new Claim("scope", scope));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.Jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _setting.Jwt.Issuer,
                _setting.Jwt.Audience,
                claims,
                expires: DateTime.Now.AddHours(_setting.Jwt.RefreshTokenExpireInDays),
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
