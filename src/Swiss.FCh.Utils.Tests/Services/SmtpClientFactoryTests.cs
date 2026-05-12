using Swiss.FCh.Utils.Services;
using MailKit.Net.Smtp;

namespace Swiss.FCh.Utils.Tests.Services;

[TestFixture]
internal sealed class SmtpClientFactoryTests
{
    [Test]
    public void Create_ShouldCreateClient()
    {
        var factory = new SmtpClientFactory();

        var client = factory.Create();

        Assert.That(client, Is.InstanceOf<ISmtpClient>());
    }
}
