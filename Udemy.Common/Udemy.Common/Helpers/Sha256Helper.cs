namespace Udemy.Common.Helpers;

public static class Sha256Helper
{
    public static string Hash(string input, string? salt)
    {
        if(!string.IsNullOrEmpty(salt)) input += salt;

        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }

    public static bool Verify(string input, string? salt, string hash)
    {
        return Hash(input, salt) == hash;
    }
}