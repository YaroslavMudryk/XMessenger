namespace XMessenger.Identity.Dtos
{
    public class MFADto
    {
        public string ManualEntryKey { get; set; }
        public string QrCodeImage { get; set; }
        public string[] RestoreCodes { get; set; }
    }
}