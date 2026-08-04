using MailKit.Net.Smtp;

namespace Swiss.FCh.Utils.Services;

/// <summary>
/// Factory that can create an <see cref="MailKit.Net.Smtp.ISmtpClient"/> for use with <see cref="Swiss.FCh.Utils.Services.IEmailService"/>.
/// </summary>
public interface ISmtpClientFactory
{
    /// <summary>
    /// Creates an <see cref="MailKit.Net.Smtp.ISmtpClient"/> for use with <see cref="Swiss.FCh.Utils.Services.IEmailService"/>
    /// </summary>
    /// <returns>The configured <see cref="MailKit.Net.Smtp.ISmtpClient"/></returns>
    ISmtpClient Create();
}
