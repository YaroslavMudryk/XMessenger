namespace XMessenger.Identity.Dtos
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public string Question { get; set; }
        public string KeyForPassword { get; set; }

        public string UserName { get; set; }
    }
}