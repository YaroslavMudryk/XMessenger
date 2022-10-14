namespace XMessenger.Domain.Models.Identity
{
    public class User : BaseModel<int>
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public bool CanBlock { get; set; }
        public DateTime? BlockedUntil { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsConfirmed { get; set; }
        public List<Session> Sessions { get; set; }
        public List<Password> Passwords { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<Qr> Qrs { get; set; }
        public List<Confirm> Confirms { get; set; }
    }
}