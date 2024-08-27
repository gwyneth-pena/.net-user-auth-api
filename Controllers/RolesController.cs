using auth.DTOs;
using auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
     ) : ControllerBase
    {

        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;


        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleDTO role)
        {
            var isExistinRole = await _roleManager.RoleExistsAsync(role.RoleName);

            if (isExistinRole) {
                return BadRequest(
                    new AuthResponseDTO
                    {
                        Message = "Role already exists."
                    }
                );
            }
            else
            {
                var createdRole =  await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
                if (!createdRole.Succeeded)
                {
                   return new ObjectResult(new { StatusCode = 500, Content = "An error occurred upon creating a role." });

                }

                return Ok(new AuthResponseDTO
                {
                    Message = "Role created successfully.",
                    IsSuccess = true
                });
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<RoleResultDTO>>> GetRoles()
        {
            var roles = await _roleManager.Roles.Select(role => new RoleResultDTO
            {
                Id = role.Id,
                Name = role.Name!,
                TotalUsers = _userManager.GetUsersInRoleAsync(role.Name!).Result.Count
            }).ToListAsync<RoleResultDTO>();

            return Ok(roles);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(string id)
        {

            if (id == null)
            {
                return BadRequest(new AuthResponseDTO {
                    Message = "Id is required."
                });
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    Message = "Role not found."
                });
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return new ObjectResult(new { StatusCode = 500, Content = "An error occurred upon deleting a role." });
            }
            return Ok(new AuthResponseDTO
            {
                Message = "Role deleted successfully!",
                IsSuccess = true
            });
        }

        [HttpPost("/assign")]
        public async Task<ActionResult> AssignRole(AssignRoleDTO assignRole) {

            var role = await _roleManager.FindByNameAsync(assignRole.RoleName);
            var user = await _userManager.FindByIdAsync(assignRole.UserId);

            if (role == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    Message = "Role not found."
                });
            }

            if (user == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    Message = "User not found."
                });
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!result.Succeeded)
            {
                return new ObjectResult(new { StatusCode = 200, Content = String.Format("User is already assigned as {0}.", role.Name) });
            }

            return Ok(new AuthResponseDTO
            {
                Message = String.Format("User is now assigned as {0}.", role.Name),
                IsSuccess = true
            });
        }
    
    }
}
