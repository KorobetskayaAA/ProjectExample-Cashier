using CashierWebApi.BL;
using CashierWebApi.BL.Exceptions;
using CashierWebApi.BL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CashierWebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService userService;

        public UsersController(UsersService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserProfileApiDto>> Get()
        {
            return await userService.GetProfiles();
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<UserProfileApiDto>> Get(string userName)
        {
            var profile = await userService.GetProfile(userName);
            if (profile == null)
            {
                return NotFound();
            }
            return profile;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserProfileCreateApiDto profile)
        {
            var result = await userService.Create(profile);
            if (result is KeyNotFoundException)
            {
                return NotFound(result.Message);
            }
            if (result is AlreadyExistsException)
            {
                return Conflict(result.Message);
            }
            if (result != null)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UserProfileCreateApiDto profile)
        {
            var result = await userService.UpdateProfile(profile);
            if (result is KeyNotFoundException)
            {
                return NotFound(result.Message);
            }
            if (result != null)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult> Delete(string userName)
        {
            var result = await userService.Delete(userName);
            if (result is KeyNotFoundException)
            {
                return NotFound(result.Message);
            }
            if (result != null)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpPost("{username}/role/{role}")]
        public async Task<ActionResult> PostRole(string userName, string role)
        {
            var result = await userService.AssignRole(userName, role);
            if (result is KeyNotFoundException)
            {
                return NotFound(result.Message);
            }
            if (result != null)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }

        [HttpDelete("{username}/role/{role}")]
        public async Task<ActionResult> DeleteRole(string userName, string role)
        {
            var result = await userService.RemoveFromRole(userName, role);
            if (result is KeyNotFoundException)
            {
                return NotFound(result.Message);
            }
            if (result != null)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
    }
}
