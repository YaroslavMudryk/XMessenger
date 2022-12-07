using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace XMessenger.Helpers
{
    public class TokenOptions
    {
        public const string Issuer = "XMessenger ID";
        public const string Audience = "XMessenger Client";
        const string SecretKey = "418b66dc05744f90b3fdc246ecc7dd372ca30e632f1744649f1df0cef16e2222"; // temp
        public const int LifeTimeInMinutes = 180;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
    }
}