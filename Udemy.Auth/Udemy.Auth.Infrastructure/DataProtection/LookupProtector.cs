using Microsoft.AspNetCore.Identity;
using System;
using System.Text;

namespace Udemy.Auth.Infrastructure.DataProtection;

public class LookupProtector(ILookupProtectorKeyRing keyRing) : ILookupProtector
{
    private readonly ILookupProtectorKeyRing _keyRing = keyRing ?? throw new ArgumentNullException(nameof(keyRing));

    public string? Protect(string keyId, string? data)
    {
        if (string.IsNullOrEmpty(data))
            return null;

        var key = _keyRing[keyId];
        var keyAndData = $"{key}:{data}";

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(keyAndData));
    }

    public string? Unprotect(string keyId, string? data)
    {
        if (string.IsNullOrEmpty(data))
            return null;

        try
        {
            var decodedData = Encoding.UTF8.GetString(Convert.FromBase64String(data));
            var separatorIndex = decodedData.IndexOf(':');

            if (separatorIndex == -1)
                return null;

            var key = decodedData[..separatorIndex];
            var originalData = decodedData[(separatorIndex + 1)..];

            if (key != _keyRing[keyId])
                return null;

            return originalData;
        }
        catch
        {
            return null;
        }
    }
}