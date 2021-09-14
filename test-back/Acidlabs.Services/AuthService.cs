using Acidlabs.Core;
using Acidlabs.Core.DTO;
using Acidlabs.Core.Repository;
using Acidlabs.Core.Services;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Acidlabs.Application
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<TokenDTO> UserLoginAsync(UserLoginDTO credentials)
        {
            TokenDTO resp = new TokenDTO();
            var user = await _userRepository.GetUserByEmailAsync(credentials.Email.ToLower());
            if (user == null) throw new Exception("No existe usuario");

            // validar clave

            resp.access_token = generateToken(user);
            return resp;
        }

        public async Task<TokenDTO> UserLoginGoogleAsync(GoogleAuthDTO credentials)
        {
            var resp = new TokenDTO();
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
            // TODO: parametrizar
            settings.Audience = new List<string>() { "481634338205-vs2eve94at5qtmk0089ukhkb0msh5bq1.apps.googleusercontent.com" };

            GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(credentials.token, settings).Result;

            var user = await _userRepository.GetUserByEmailAsync(payload.Email.ToLower().Trim());

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = payload.Name,
                    Email = payload.Email.ToLower().Trim()
                };
                await _userRepository.SaveUserAsync(user);
            }
            resp.access_token = generateToken(user);
            return resp;
        }

        private string generateToken(User user)
        {
            var claims = new[] {
                new Claim (JwtRegisteredClaimNames.Sub, user.Name ),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            //TODO: Cambiar a parametro
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("TestAcidLabs - 1237jfafa890kasñsdl90"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(20),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
