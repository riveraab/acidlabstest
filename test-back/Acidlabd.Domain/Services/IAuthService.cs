using Acidlabs.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acidlabs.Core.Services
{
    public interface IAuthService
    {
        Task<TokenDTO> UserLoginAsync(UserLoginDTO credentials);
        Task<TokenDTO> UserLoginGoogleAsync(GoogleAuthDTO credentials);
    }
}
