using Swiss.FCh.Utils.Configurations;
using Swiss.FCh.Utils.Models;
using Swiss.FCh.Utils.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Swiss.FCh.Utils.Tests.Services;

[TestFixture]
internal sealed class EmailServiceTests
{
    private readonly ILogger<EmailService> _logger = NullLogger<EmailService>.Instance;
    private readonly ISmtpClientFactory _smtpClientFactory = Substitute.For<ISmtpClientFactory>();
    private readonly IOptions<EmailServiceOptions> _emailOptions = Substitute.For<IOptions<EmailServiceOptions>>();
    private readonly ISmtpClient _smtpClient = Substitute.For<ISmtpClient>();
    private readonly EmailServiceOptions _emailOptionsValue = new()
    {
        Host = "test_host",
        Port = 1234
    };

    private readonly Email _email = new()
    {
        From = new EmailAddress { Name = "test_from_name", Address = "test_from_address" },
        To = new[] { new EmailAddress { Name = "test_to_name", Address = "test_to_address" } },
        Subject = "test_subject",
        TextMessage = "test_message"
    };

    private EmailService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _smtpClientFactory.Create().Returns(_smtpClient);
        _emailOptions.Value.Returns(_emailOptionsValue);

        _service = new EmailService(_logger, _smtpClientFactory, _emailOptions);
    }

    [TearDown]
    public void TearDown()
    {
        _smtpClient.ClearSubstitute();
        _emailOptions.ClearSubstitute();
        _smtpClientFactory.ClearSubstitute();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _smtpClient.Dispose();
    }

    [Test]
    public async Task Send_Email_ShouldSendEmail()
    {
        MimeMessage messageToSend = null!;
        _smtpClient.SendAsync(Arg.Do<MimeMessage>(m => messageToSend = m)).Returns(string.Empty);

        await _service.Send(_email).ConfigureAwait(false);

        _smtpClientFactory.Received(1).Create();
        await _smtpClient.Received(1).ConnectAsync(Arg.Is(_emailOptionsValue.Host), Arg.Is(_emailOptionsValue.Port)).ConfigureAwait(false);
        await _smtpClient.Received(1).SendAsync(Arg.Is(messageToSend)).ConfigureAwait(false);
        await _smtpClient.Received(1).DisconnectAsync(Arg.Is(true)).ConfigureAwait(false);
    }

    [TestCase]
    public void Send_ThrowingException_ShouldRethrowException()
    {
#pragma warning disable CA2201
        _smtpClient.ConnectAsync(Arg.Any<string>(), Arg.Any<int>()).ThrowsAsyncForAnyArgs(new Exception());
#pragma warning restore CA2201

        Assert.That(async () => await _service.Send(_email).ConfigureAwait(false), Throws.InstanceOf<Exception>());
    }

    [TestCase]
    public void Send_ThrowingSmtpCommandException_ShouldRethrowSmtpCommandException()
    {
        _smtpClient.ConnectAsync(Arg.Any<string>(), Arg.Any<int>()).ThrowsAsyncForAnyArgs(new SmtpCommandException(SmtpErrorCode.UnexpectedStatusCode, SmtpStatusCode.SyntaxError, string.Empty));

        Assert.That(async () => await _service.Send(_email).ConfigureAwait(false), Throws.InstanceOf<SmtpCommandException>());
    }

    [TestCase]
    public void Send_ThrowingSmtpProtocolException_ShouldRethrowSmtpProtocolException()
    {
        _smtpClient.ConnectAsync(Arg.Any<string>(), Arg.Any<int>()).ThrowsAsyncForAnyArgs(new SmtpProtocolException());

        Assert.That(async () => await _service.Send(_email).ConfigureAwait(false), Throws.InstanceOf<SmtpProtocolException>());
    }
}
