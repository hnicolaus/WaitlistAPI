using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Domain.Services
{
    public class TokenService
    {
        private readonly Authentication _authenticationConfig;
        private readonly Dictionary<TokenType, int> TokenTypeToExpiryMinutes;

        public TokenService(IOptions<Authentication> options)
        {
            _authenticationConfig = options.Value;

            TokenTypeToExpiryMinutes = new Dictionary<TokenType, int>
            {
                [TokenType.Customer] = 30,
                [TokenType.Admin] = 480
            };
        }

        public string GetToken(string userId, string clientId, TokenType tokenType)
        {
            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userId, clientId, tokenType);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor); //sign the JWT with the symmetric key
            string tokenString = tokenHandler.WriteToken(securityToken); //generate string (compact) version of JWT

            return tokenString;
        }

        private SecurityTokenDescriptor GetTokenDescriptor(string userId, string clientId, TokenType tokenType)
        {
            byte[] securityKey = Encoding.UTF8.GetBytes(_authenticationConfig.SymmetricKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(TokenTypeToExpiryMinutes[tokenType]),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
                Audience = clientId,
                Issuer = _authenticationConfig.IssuerId,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, tokenType.ToString()),
                }),
            };

            return tokenDescriptor;
        }
    }
}