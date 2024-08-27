using System.ComponentModel.DataAnnotations;

namespace auth.DTOs
{
    public class CreateRoleDTO
    {
        [Required]
        public required string RoleName { get; set; }
    }
}
