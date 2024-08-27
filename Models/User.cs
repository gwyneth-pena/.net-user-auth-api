using Microsoft.AspNetCore.Identity;

namespace auth.Models
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
