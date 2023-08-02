using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.DTOs
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    } 
}