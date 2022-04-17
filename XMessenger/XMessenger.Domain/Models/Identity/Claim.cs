using System.ComponentModel.DataAnnotations;

namespace XMessenger.Domain.Models.Identity
{
    public class Claim : BaseModel<int>
    {
        [Required, StringLength(500)]
        public string Type { get; set; }
        [Required, StringLength(500)]
        public string Value { get; set; }
        [StringLength(500, MinimumLength = 1)]
        public string DisplayText { get; set; }
        public List<RoleClaim> RoleClaims { get; set; }
        public List<AppClaim> AppClaims{ get; set; }
    }
}