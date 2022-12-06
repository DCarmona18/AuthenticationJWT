using AuthenticationJWT.Domain.Interfaces;
using AuthenticationJWT.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationJWT.Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly TimeSpan _expiryDuration = new (0, 30, 0);
        private readonly IUserRepository _userRepository;

        public TokenService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> BuildToken(string key, string issuer, IEnumerable<string> audience, string userName, string password)
        {
            var user = await _userRepository.GetUserByUsernameAndPassword(userName, password);
            if (user == null)
                throw new Exception("User not found");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim(System.Security.Claims.ClaimTypes.Role , user.Role)
            };

            claims.AddRange(audience.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.Add(_expiryDuration), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
