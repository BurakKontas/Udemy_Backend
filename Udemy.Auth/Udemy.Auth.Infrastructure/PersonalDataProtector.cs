using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Udemy.Auth.Infrastructure;

public class PersonalDataProtector : IPersonalDataProtector
{
    public string Protect(string? data)
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
    }

    public string Unprotect(string? protectedData)
    {
        if (string.IsNullOrEmpty(protectedData))
            return string.Empty;

        try
        {
            var result = Encoding.UTF8.GetString(Convert.FromBase64String(protectedData));;
            return result;
        }
        catch
        {
            return string.Empty;
        }
    }
}
