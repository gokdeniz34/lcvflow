using System.Security.Cryptography;
using System.Text;

namespace LcvFlow.Service.Helpers.Auth;

public static class PasswordHelper
{
    public static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public static bool Verify(string hashedPassword, string providedPassword)
    {
        return Hash(providedPassword) == hashedPassword;
    }
}