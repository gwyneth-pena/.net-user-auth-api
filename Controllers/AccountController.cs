using auth.DTOs;
using auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using auth.Shared;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        Utils utils
        ) : ControllerBase
    {

        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly Utils _utils = utils;

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                FullName = registerDTO.FullName,
                Email = registerDTO.EmailAddress,
                UserName = registerDTO.EmailAddress,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (registerDTO.Roles == null)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                foreach (var role in registerDTO.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Account created successfully!"
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDTO loginDTO) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDTO.EmailAddress);

            if (user == null)
            {
                return Unauthorized(new AuthResponseDTO
                {
                    Message = "Invalid credentials.",
                    IsSuccess = false
                });
            }

            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);


            if (!isCorrectPassword)
            {
                return Unauthorized(new AuthResponseDTO
                {
                    Message = "Invalid credentials.",
                    IsSuccess = false
                });
            }


            return Ok(new AuthResponseDTO
            {
                Message = "Login successfully!",
                IsSuccess = true,
                Token = _utils.generateToken(user)
            });
        }
    }
}
