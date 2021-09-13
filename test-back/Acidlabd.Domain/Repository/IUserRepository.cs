using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acidlabs.Core.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task SaveUserAsync(User user);
        Task<User> getUserByIdAsync(string id);
    }
}
