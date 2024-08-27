namespace auth.DTOs
{
    public class AuthResponseDTO
    {

        public string? Token { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; }

    }
}
