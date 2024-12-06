using Microsoft.AspNetCore.Identity;
using Udemy.Auth.Domain;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Auth.Infrastructure;

public class DataProtectionProvider : IUserTwoFactorTokenProvider<User>
{
    public async Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
    {
        // Kullanıcı ID'si, amacınız ve bir zaman damgasına dayalı olarak bir token oluşturun
        var data = $"{user.Id}:{purpose}:{DateTime.UtcNow.Ticks}";
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        return await Task.FromResult(token);
    }

    public async Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
    {
        try
        {
            // Token'ı çöz
            var data = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var parts = data.Split(':');

            // Kullanıcı ID'si ve amacını doğrula
            if (parts.Length != 3 || parts[0] != user.Id || parts[1] != purpose)
                return await Task.FromResult(false);

            // Token'in süresini kontrol et (örneğin 5 dakika geçerli)
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

    public async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
    {
        // Bu yöntemde, kullanıcı için iki faktörlü doğrulamanın desteklenip desteklenmediğini belirtebilirsiniz
        // Örneğin, e-posta doğrulama etkinse true dönebilir
        return await Task.FromResult(!string.IsNullOrEmpty(user.Email));
    }
}