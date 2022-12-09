﻿namespace XMessenger.Domain.Models.Identity
{
    public class LoginChange : BaseModel<int>
    {
        public string OldLogin { get; set; }
        public string NewLogin { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}