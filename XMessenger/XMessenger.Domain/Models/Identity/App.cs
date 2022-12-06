using System.ComponentModel.DataAnnotations;

namespace XMessenger.Domain.Models.Identity
{
    public class App: BaseModel<int>
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public List<AppClaim> AppClaims { get; set; }

        public bool IsActiveByTime()
        {
            var now = DateTime.Now;
            return now >= ActiveFrom && now <= ActiveTo;
        }
    }
}
