using System.Text;

namespace FinanceApplication.Core.Security;

public class HashingHelper
{

    public static void CreatePasswordHash(string password, out byte[] passwordsalt, out byte[] passwordhash)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordsalt = hmac.Key;
            passwordhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }


    public static bool VerifyPasswordHash(string password, byte[] passwordsalt, byte[] passwordhash)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordsalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordhash[i])
                {
                    return false;
                }
            }
        }
        return true;
    }

}