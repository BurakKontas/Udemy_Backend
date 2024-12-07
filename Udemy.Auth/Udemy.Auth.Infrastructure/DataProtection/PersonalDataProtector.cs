using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;
using Udemy.Common.Helpers;

namespace Udemy.Auth.Infrastructure.DataProtection;

public class PersonalDataProtector : IPersonalDataProtector
{
    private const string Key = "REquIJQA9V2C8xvFp9zDkd3YF5xBE9hr";
    private const string Iv = "4fRQqeydqThPh6lm";
    private static readonly AesEncryptionHelper Aes = new(Key, Iv);

    public string Protect(string? data)
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        return Aes.Encrypt(data);
    }

    public string Unprotect(string? protectedData)
    {
        if (string.IsNullOrEmpty(protectedData))
            return string.Empty;

        try
        {
            return Aes.Decrypt(protectedData);
        }
        catch
        {
            return string.Empty;
        }
    }
}
