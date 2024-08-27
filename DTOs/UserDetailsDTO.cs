namespace auth.DTOs
{
    public class UserDetailsDTO
    {

        public string? Username { set; get; }
        public string? FullName { set; get; }
        public string? Email { set; get; }
        public string? PhoneNumber { set; get; }
        public bool? EmailConfirmed { set; get; }
        public List<string>? Roles { get; set; }
        public int? AccessFailedCount { set; get; }


    }
}
