using Microsoft.AspNetCore.Identity;
using Udemy.Auth.Domain;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Auth.Infrastructure.DataProtection;

public class DataProtectionProvider : IUserTwoFactorTokenProvider<Domain.User>
{
    public async Task<string> GenerateAsync(string purpose, UserManager<Domain.User> manager, Domain.User user)
    {
        var data = $"{user.Id}:{purpose}:{DateTime.UtcNow.Ticks}";
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        return await Task.FromResult(token);
    }

    public async Task<bool> ValidateAsync(string purpose, string token, UserManager<Domain.User> manager, Domain.User user)
    {
        try
        {
            var data = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var parts = data.Split(':');

            if (parts.Length != 3 || parts[0] != user.Id || parts[1] != purpose)
                return await Task.FromResult(false);

            var ticks = long.Parse(parts[2]);
            var tokenTime = new DateTime(ticks);
            var isValid = (DateTime.UtcNow - tokenTime).TotalMinutes <= 5;

            return await Task.FromResult(isValid);
        }
        catch
        {
            return await Task.FromResult(false);
        }
    }

    public async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<Domain.User> manager, Domain.User user)
    {
        return await Task.FromResult(!string.IsNullOrEmpty(user.Email));
    }
}