using IdentityWebApp.Other.Settings;
using IdentityWebApp.Services.Creators;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace IdentityWebApp.Services.Senders;

/// <summary>
/// Отправитель письма по электронной почте по протоколу SMTP.
/// </summary>
public class SmtpEmailSender : IEmailSender
{  
    private readonly SmtpSettings _smtpSettings;
    
    public SmtpEmailSender(IOptions<SmtpSettings> smtpSettings)
    {
         _smtpSettings = smtpSettings.Value;
    }

    /// <inheritdoc/>
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtpClient =  SmtpClientCreator.Create(_smtpSettings);
        await smtpClient.SendMailAsync(_smtpSettings.Username, email, subject, htmlMessage);
    }
}
