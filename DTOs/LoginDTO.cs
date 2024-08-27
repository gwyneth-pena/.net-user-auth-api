using System.ComponentModel.DataAnnotations;

namespace auth.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public required string EmailAddress { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
