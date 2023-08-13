namespace IdentityService.Application.DTOs
{
    public class AuthorizationDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}