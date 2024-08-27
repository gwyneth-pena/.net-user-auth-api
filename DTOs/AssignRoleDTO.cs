using System.ComponentModel.DataAnnotations;

namespace auth.DTOs
{
    public class AssignRoleDTO
    {
        [Required]
        public required string UserId { get; set; }


        [Required]
        public required string RoleName { get; set; }
    }
}
