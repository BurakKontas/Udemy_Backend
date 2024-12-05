namespace Udemy.Auth.Infrastructure;

using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class PersonalDataProtector : IPersonalDataProtector
{
    public string Protect(string? data)
    {
        if(string.IsNullOrEmpty(data))         
        {
            return string.Empty;
        }

        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
    }

    public string Unprotect(string? protectedData)
    {
        if (string.IsNullOrEmpty(protectedData))
        {
            return string.Empty;
        }

        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(protectedData));
    }
}
