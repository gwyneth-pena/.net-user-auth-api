using System.ComponentModel.DataAnnotations;

namespace auth.DTOs
{
    public class RegisterDTO
    {
        [EmailAddress]
        [Required]
        public required string EmailAddress { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string FullName { get; set; }

        public List<string>? Roles { get; set; }

    }
}
