using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace TodoBackend.Utils
{
    public static class TokenUtils
    {
        private const string SecretKey = "8893827s7d6ggwk3hl266h3k2j34h_random_str_5549fgwth";
        private const int ExpiryMinutes = 60;

        public static string GenerateToken(string userId)
        {
            var key = Encoding.UTF8.GetBytes(SecretKey);

            if (key.Length < 32)
                throw new ArgumentOutOfRangeException(nameof(key), "Key must be at least 256 bits (32 bytes).");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(ExpiryMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
