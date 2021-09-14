using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Acidlabs.Core.DTO;
using Acidlabs.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Acidlabs.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO credentials)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var tokenObj = await _authService.UserLoginAsync(credentials);
                if (tokenObj != null)
                {
                    return Ok(tokenObj);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [HttpPost("login/google")]
        public async Task<IActionResult> ExternalLogin([FromBody] GoogleAuthDTO googleSignIn)
        {
            try
            {
                var tokenObj = await this._authService.UserLoginGoogleAsync(googleSignIn);
                return Ok(tokenObj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }           
        }
    }
}
