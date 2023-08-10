using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.DTOs
{
    public class LoginUserDto
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    } 
}