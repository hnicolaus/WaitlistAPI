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
        private readonly Authentication _authenticationConfig;
        private const int _expiryMinutes = 60;

        public TokenService(IOptions<Authentication> options)
        {
            _authenticationConfig = options.Value;
        }

        public string GetToken(string userId, string clientId)
        {
            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userId, clientId);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor); //sign the JWT with the symmetric key
            string tokenString = tokenHandler.WriteToken(securityToken); //generate string (compact) version of JWT

            return tokenString;
        }

        private SecurityTokenDescriptor GetTokenDescriptor(string userId, string clientId)
        {
            byte[] securityKey = Encoding.UTF8.GetBytes(_authenticationConfig.SymmetricKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
                Audience = clientId,
                Issuer = _authenticationConfig.IssuerId,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
            };

            return tokenDescriptor;
        }
    }
}