using Swiss.FCh.Utils.Configurations;
using Swiss.FCh.Utils.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Swiss.FCh.Utils.Services;

internal class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly ISmtpClientFactory _smtpClientFactory;
    private readonly EmailServiceOptions _emailOptions;

    public EmailService(ILogger<EmailService> logger, ISmtpClientFactory smtpClientFactory, IOptions<EmailServiceOptions> emailOptions)
    {
        ArgumentNullException.ThrowIfNull(emailOptions);

        _logger = logger;
        _smtpClientFactory = smtpClientFactory;
        _emailOptions = emailOptions.Value;
    }

    public async Task Send(Email email)
    {
        ArgumentNullException.ThrowIfNull(email);

        try
        {
            var message = new MimeMessage();
            var bodyBuilder = new BodyBuilder();

            if (!string.IsNullOrWhiteSpace(email.HtmlMessage))
            {
                bodyBuilder.HtmlBody = email.HtmlMessage;
            }
            else
            {
                bodyBuilder.TextBody = email.TextMessage;
            }

            message.From.Add(new MailboxAddress(email.From.Name, email.From.Address));
            foreach (var to in email.To)
            {
                message.To.Add(new MailboxAddress(to.Name, to.Address));
            }

            message.Subject = email.Subject;

            message.Body = bodyBuilder.ToMessageBody();

            using var client = _smtpClientFactory.Create();
            await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            message.Dispose();
        }
        catch (SmtpCommandException e)
        {
            // The exception that is thrown when an SMTP command fails.
            // Unlike a SmtpProtocolException, a SmtpCommandException does not require the SmtpClient to be reconnected.
            _logger.LogError(e, "Sending mail failed: MailKitErrorCode: {ErrorCode} | SmtpStatusCode: {StatusCode} | Mailbox: {Address} | Message: {Message}", e.ErrorCode, e.StatusCode, e.Mailbox?.Address, e.Message);
            throw;
        }
        catch (SmtpProtocolException e)
        {
            // The exception that is thrown when there is an error communicating with an SMTP server.
            // An SmtpProtocolException is typically fatal and requires the SmtpClient to be reconnected.
            _logger.LogError(e, "Sending mail failed: error communicating with the SMTP server: {Message}", e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Sending mail failed: {Message}", e.Message);
            throw;
        }
    }
}
