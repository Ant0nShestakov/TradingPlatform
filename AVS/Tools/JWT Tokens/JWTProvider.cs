using AVS.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AVS.Tools.JWT_Tokens
{
    public class JWTProvider : IJWTProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JWTProvider(IOptions<JwtOptions> options) 
        {
            this._jwtOptions = options.Value;
        }

        public string GenerateToken(User user)
        {
            Claim[] claims =
            [
                new("user_id", user.Id.ToString()),
                new("LoginUser", "true")
            ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpitesHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
