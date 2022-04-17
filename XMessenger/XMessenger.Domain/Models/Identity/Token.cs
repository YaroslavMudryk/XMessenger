﻿using System.ComponentModel.DataAnnotations;

namespace XMessenger.Domain.Models.Identity
{
    public class Token : BaseModel<long>
    {
        [Required, StringLength(5000, MinimumLength = 10)]
        public string JwtToken { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime ExpiredAt { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
    }
}