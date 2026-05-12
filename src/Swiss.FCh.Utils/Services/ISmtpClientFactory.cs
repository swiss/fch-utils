using MailKit.Net.Smtp;

namespace Swiss.FCh.Utils.Services;

public interface ISmtpClientFactory
{
    ISmtpClient Create();
}
