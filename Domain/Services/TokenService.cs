using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Domain.Services
{
    public class TokenService
    {
        private Encryption encryptionSettings;
        private const int _expiryMinutes = 60;

        public TokenService(IOptions<Encryption> options)
        {
            encryptionSettings = options.Value;
        }

        public string GetToken(string userId)
        {
            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userId);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor); //sign the JWT with the symmetric key
            string tokenString = tokenHandler.WriteToken(securityToken); //generate string (compact) version of JWT

            return tokenString;
        }

        private SecurityTokenDescriptor GetTokenDescriptor(string userId)
        {
            byte[] securityKey = Encoding.UTF8.GetBytes(encryptionSettings.SymmetricKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
                Audience = "WaitlistApplication", //front-end
                Issuer = "WaitlistAPI",
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
            };

            return tokenDescriptor;
        }
    }
}