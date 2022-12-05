using Extensions.DeviceDetector.Models;
using System.ComponentModel.DataAnnotations;
using XMessenger.Helpers;

namespace XMessenger.Application.Dtos.Identity
{
    public class LoginDto
    {
        [Required, EmailAddress, StringLength(200, MinimumLength = 5)]
        public string Login { get; set; }
        [Required]
        [RegularExpression(RegexTemplate.Password.Regex, ErrorMessage = RegexTemplate.Password.ErrorMessage)]
        public string Password { get; set; }
        [Required]
        public string Lang { get; set; }
        public ClientInfo Client { get; set; }
        public AppLoginDto App { get; set; }
    }
}
