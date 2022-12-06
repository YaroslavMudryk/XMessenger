namespace XMessenger.Helpers
{
    public class Generator
    {
        public static string[] GetRestoreCodes()
        {
            var codes = new string[8];

            for (int i = 0; i < 8; i++)
            {
                codes[i] = Guid.NewGuid().ToString("N").ToUpper().Substring(8);
            }

            return codes;
        }

        public static string GetConfirmCode()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
