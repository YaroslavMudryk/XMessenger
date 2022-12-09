using System.ComponentModel.DataAnnotations;

namespace XMessenger.Identity.Dtos
{
    public class NewLoginDto
    {
        [Required, EmailAddress]
        public string NewLogin { get; set; }
        public string CodeMFA { get; set; }
    }
}