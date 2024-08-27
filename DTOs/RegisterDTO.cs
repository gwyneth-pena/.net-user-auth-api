using System.ComponentModel.DataAnnotations;

namespace auth.DTOs
{
    public class RegisterDTO
    {
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public required List<string> Roles { get; set; }

    }
}
