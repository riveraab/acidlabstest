using Acidlabs.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acidlabs.Core.Services
{
    public interface IUserService
    {
        Task<IList<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> SaveUserAsync(UserRegisterDTO user);
        Task<User> UpdateUserAsync(string id, UserRegisterDTO user);
        Task DeleteUserAsync(string id);
    }
}
