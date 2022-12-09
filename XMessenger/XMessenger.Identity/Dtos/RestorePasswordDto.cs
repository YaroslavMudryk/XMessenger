namespace XMessenger.Identity.Dtos
{
    public class RestorePasswordDto
    {
        public string Login { get; set; }
        public string NewPassword { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Code { get; set; }
    }
}