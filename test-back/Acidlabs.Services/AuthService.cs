﻿using Acidlabs.Core;
using Acidlabs.Core.DTO;
using Acidlabs.Core.Repository;
using Acidlabs.Core.Services;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;
        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
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

            string clientId = _config.GetValue<string>("Google:ClientId");
            settings.Audience = new List<string>() { clientId };

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

            string keyCfg = _config.GetValue<string>("Authentication:Token");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(keyCfg));
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
