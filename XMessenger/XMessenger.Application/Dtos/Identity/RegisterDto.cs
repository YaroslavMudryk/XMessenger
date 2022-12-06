namespace XMessenger.Application.Dtos.Identity
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public string KeyForPassword { get; set; }

        public string UserName { get; set; }
    }
}