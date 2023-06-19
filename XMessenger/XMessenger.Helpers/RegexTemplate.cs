namespace XMessenger.Helpers
{
    public static class RegexTemplate
    {
        public static class Password
        {
            public const string Regex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
            public const string ErrorMessage = "Мінімум вісім символів, принаймні одна літера, одна цифра та один спеціальний символ ($!%*#?&)";
        }
    }
}
