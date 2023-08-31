using System.ComponentModel.DataAnnotations;

namespace Identity.Application.DTOs
{
    public class LoginUserDto
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    } 
}