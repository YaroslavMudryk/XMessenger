﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMessenger.Domain.Models.Identity
{
    public class Password : BaseModel<int>
    {
        public string PasswordHash { get; set; }

        public string Answer { get; set; }

        public string Question { get; set; }

        public bool IsActive { get; set; }

        public DateTime DeactivatedAt { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}