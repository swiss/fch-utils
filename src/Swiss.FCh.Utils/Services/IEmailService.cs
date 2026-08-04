using Swiss.FCh.Utils.Models;

namespace Swiss.FCh.Utils.Services;

/// <summary>
/// This service can send e-mail messages over an SMTP server.
/// In order to use this service, <see cref="Swiss.FCh.Utils.Extensions.UtilsServiceCollectionExtensions.AddEmailService"/> has to be called to register the service in the DI container.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends the e-mail over an SMTP server.
    /// </summary>
    /// <param name="email">The e-mail message that is sent</param>
    /// <returns>Asynchronous task to be awaited</returns>
    Task Send(Email email);
}
