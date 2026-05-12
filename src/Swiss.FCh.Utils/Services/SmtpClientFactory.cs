using MailKit.Net.Smtp;

namespace Swiss.FCh.Utils.Services;

public class SmtpClientFactory : ISmtpClientFactory
{
    public ISmtpClient Create()
    {
        return new SmtpClient
        {
            CheckCertificateRevocation = false // using the check certificate revocation option can lead to memory leaks (https://github.com/jstedfast/MailKit/issues/1105)
        };
    }
}
