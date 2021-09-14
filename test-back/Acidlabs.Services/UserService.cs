using Acidlabs.Core;
using Acidlabs.Core.DTO;
using Acidlabs.Core.Repository;
using Acidlabs.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acidlabs.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> SaveUserAsync(UserRegisterDTO user)
        {
            var usr = new User
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = user.Name,
                Email = user.Email
            };

            return await _userRepository.SaveUserAsync(usr);
        }


        public async Task<User> UpdateUserAsync(string id, UserRegisterDTO user)
        {
            var usr = await _userRepository.GetUserByIdAsync(id);
            if (usr == null) throw new Exception("Usuario no existe");
            usr.Name = user.Name;
            usr.Email = user.Email;
            //TODO: Cambiar password y verificar correo utilizado
            return await _userRepository.SaveUserAsync(usr);
        }


        public async Task DeleteUserAsync(string id)
        {
            var usr = await _userRepository.GetUserByIdAsync(id);
            if (usr == null) throw new Exception("Usuario no existe");

            await _userRepository.RemoveUserAsync(id);
        }


    }
}
