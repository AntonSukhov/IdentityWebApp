using System.Net.Mail;
using IdentityWebApp.Other.Settings;

namespace IdentityWebApp.Services.Creators;

public static class SmtpClientCreator
{
    public static SmtpClient Create(SmtpSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return new SmtpClient
        {
            Port = settings.Port,
            Host = settings.Host,
            DeliveryMethod = settings.DeliveryMethod,
            UseDefaultCredentials = settings.UseDefaultCredentials
        };
    }
}
