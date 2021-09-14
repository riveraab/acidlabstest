using Acidlabs.Core;
using Acidlabs.Core.DTO;
using Acidlabs.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acidlabs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userService.GetUsersAsync();

                return Ok(new { users = users });

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user != null)
                {
                    return Ok(user);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRegisterDTO user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var created = await _userService.SaveUserAsync(user);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }


        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserRegisterDTO user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var created = await _userService.UpdateUserAsync(id, user);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }




    }
}
