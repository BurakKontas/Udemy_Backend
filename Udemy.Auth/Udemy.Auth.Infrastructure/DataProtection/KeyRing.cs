using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Udemy.Auth.Infrastructure.DataProtection;

public class KeyRing : ILookupProtectorKeyRing
{
    private readonly Dictionary<string, string> _keyStore;

    public KeyRing()
    {
        _keyStore = new Dictionary<string, string>();

        var keyId = "defaultKeyId";
        var key = "12345678901234567890123456789012";
        _keyStore[keyId] = key;
        CurrentKeyId = keyId;
    }

    public string CurrentKeyId { get; }

    public IEnumerable<string> GetAllKeyIds()
    {
        return _keyStore.Keys;
    }

    public string this[string keyId]
    {
        get
        {
            if (_keyStore.TryGetValue(keyId, out var key))
            {
                return key;
            }

            throw new KeyNotFoundException($"Anahtar ID '{keyId}' bulunamadı.");
        }
    }
}