using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace IOT.Repositories
{
    public interface ITokenRepository
    {
        string GenerateToken(string UserName, string MobileNumber);
    }


    public class TokenRepository : ITokenRepository
    {

        private readonly IConfiguration _config;

        public TokenRepository(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string UserName, string MobileNumber)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

            // 1. Define Claims (payload stored in token)
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Name, UserName),
            new Claim(JwtRegisteredClaimNames.PhoneNumber, MobileNumber),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            // 2. Define Signing Credentials
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. Create the Token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
                signingCredentials: credentials
            );

            // 4. Return serialized string token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
