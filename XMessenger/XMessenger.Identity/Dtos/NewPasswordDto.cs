namespace XMessenger.Identity.Dtos
{
    public class NewPasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string CodeMFA { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}