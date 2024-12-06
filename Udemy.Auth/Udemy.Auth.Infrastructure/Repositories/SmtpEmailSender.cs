using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Udemy.Auth.Infrastructure.Repositories;

public class SmtpEmailSender(SmtpClient smtpClient) : IEmailSender<Domain.Entities.User>
{
    private readonly SmtpClient _smtpClient = smtpClient;
    private const string DefaultEmail = "noreply@kontas.tr";

    public async Task SendConfirmationLinkAsync(Domain.Entities.User user, string email, string confirmationLink)
    {
        var subject = "Confirm Your Email";
        var body = $"Hello {user.UserName},\n\nPlease confirm your email by clicking the link below:\n{confirmationLink}";
        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetLinkAsync(Domain.Entities.User user, string email, string resetLink)
    {
        var subject = "Password Reset Request";
        var body = $"Hello {user.UserName},\n\nYou can reset your password by clicking the link below:\n{resetLink}";
        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetCodeAsync(Domain.Entities.User user, string email, string resetCode)
    {
        var subject = "Password Reset Code";
        var body = $"Hello {user.UserName},\n\nHere is your password reset code:\n{resetCode}";
        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string recipient, string subject, string body)
    {
        using var mailMessage = new MailMessage(DefaultEmail, recipient);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.BodyEncoding = Encoding.UTF8;
        mailMessage.IsBodyHtml = false;

        try
        {
            await _smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error sending email.", ex);
        }
    }
}