using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acidlabs.Core.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> SaveUserAsync(User user);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByEmailAsync(string email);
        Task RemoveUserAsync(string id);
    }
}
