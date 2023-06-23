using System.Security.Cryptography;

namespace LMSSystem.Prototypes
{
    public class jwtHandler
    {

        public jwtHandler()
        {

        }

        public static string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}