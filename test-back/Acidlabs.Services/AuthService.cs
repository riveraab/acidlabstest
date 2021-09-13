using Acidlabs.Core.DTO;
using Acidlabs.Core.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Acidlabs.Application
{
    public class AuthService : IAuthService
    {
        public async Task<TokenDTO> UserLoginAsync(UserLoginDTO credentials)
        {

            TokenDTO resp = new TokenDTO();

            var claims = new[] {
                new Claim (JwtRegisteredClaimNames.Sub, "some_id" ),
                new Claim("granny", "cookie")
            };

            //TODO: Cambiar a parametro
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TestAcidLabs - 1237jfafa890kasñsdl90"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(20),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            resp.access_token = tokenHandler.WriteToken(token);
            return resp;
        }

        public Task<bool> VerifyGoogleTokenAsync(GoogleAuthDTO credentials)
        {
            throw new NotImplementedException();
        }
    }
}
