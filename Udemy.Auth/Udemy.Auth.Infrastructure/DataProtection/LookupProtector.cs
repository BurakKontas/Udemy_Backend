using Microsoft.AspNetCore.Identity;
using System;
using System.Text;
using Udemy.Common.Helpers;

namespace Udemy.Auth.Infrastructure.DataProtection;

public class LookupProtector(ILookupProtectorKeyRing keyRing) : ILookupProtector
{
    private readonly ILookupProtectorKeyRing _keyRing = keyRing ?? throw new ArgumentNullException(nameof(keyRing));

    public string? Protect(string keyId, string? data)
    {
        if (string.IsNullOrEmpty(data))
            return null;

        var key = _keyRing[keyId];
        var aes = new AesEncryptionHelper(key, key[..16]);

        return aes.Encrypt(data);
    }

    public string? Unprotect(string keyId, string? data)
    {
        if (string.IsNullOrEmpty(data))
            return null;

        try
        {
            var key = _keyRing[keyId];
            var aes = new AesEncryptionHelper(key, key[..16]);
            return aes.Decrypt(data);
        }
        catch
        {
            return null;
        }
    }
}