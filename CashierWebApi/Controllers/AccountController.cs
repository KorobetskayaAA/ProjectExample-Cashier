using CashierDB.Model.DTO;
using CashierWebApi.BL;
using CashierWebApi.BL.Exceptions;
using CashierWebApi.BL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CashierWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UsersService userService;
        private readonly SignInManager<UserDbDto> signInManager;

        public AccountController(UsersService userService, SignInManager<UserDbDto> signInManager)
        {
            this.userService = userService;
            this.signInManager = signInManager;
        }

        public class LoginRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var result = await signInManager.PasswordSignInAsync(request.UserName, request.Password,
                request.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return Ok();
        }

        [HttpPost("logout")]
        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public class ChangePasswordRequest
        {
            public string NewPassword { get; set; }
        }

        [HttpGet]
        public UserProfileApiDto Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var profile = new UserProfileApiDto()
            {
                UserName = identity.Name,
                Email = identity.FindFirst(ClaimTypes.Email)?.Value,
                FirstName = identity.FindFirst("FirstName")?.Value,
                MiddleName = identity.FindFirst("MiddleName")?.Value,
                LastName = identity.FindFirst("LastName")?.Value,
            };
            return profile;
        }

        [HttpGet("roles")]
        public string GetRoles()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var roles = identity.FindFirst(ClaimTypes.Role)?.Value;
            return roles;
        }

        [HttpPut]
        public async Task<ActionResult> PutProfile(UserProfileApiDto profile)
        {
            if (profile.UserName != HttpContext.User.Identity.Name)
            {
                return BadRequest("Имя пользователя некорректно.");
            }
            var result = await userService.UpdateProfile(profile);
            if (result is KeyNotFoundException)
            {
                return NotFound(result.Message);
            }
            if (result != null)
            {
                return BadRequest(result.Message);
            }
            var user = await signInManager.UserManager.GetUserAsync(HttpContext.User);
            await signInManager.RefreshSignInAsync(user);
            return Ok();
        }

        [HttpPost("password")]
        public async Task<ActionResult> PostPassword([FromForm] string password)
        {
            var result = await userService.ResetPassword(HttpContext.User.Identity.Name, password);
            if (result is KeyNotFoundException)
            {
                return NotFound(result.Message);
            }
            if (result is SaveChangesException)
            {
                return BadRequest(result.Message);
            }
            if (result != null)
            {
                return StatusCode(500, result.Message);
            }
            return Ok();
        }
    }
}
