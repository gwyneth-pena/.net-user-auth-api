namespace auth.DTOs
{
    public class LoginDTO
    {
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}
