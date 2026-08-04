namespace Swiss.FCh.Utils.Models;

/// <summary>
/// This class defines the possible configuration for the <see cref="Swiss.FCh.Utils.Services.IEmailService"/>.
/// </summary>
public class Email
{
    /// <summary>
    /// The sender of the e-mail message.
    /// </summary>
    public required EmailAddress From { get; set; }

    /// <summary>
    /// The receiver e-mail address(es).
    /// </summary>
    public required IEnumerable<EmailAddress> To { get; set; }

    /// <summary>
    /// Subject of the e-mail message.
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    /// Plain text content of the e-mail message.
    /// </summary>
    public string? TextMessage { get; set; }

    /// <summary>
    /// HTML content of the e-mail message.
    /// </summary>
    public string? HtmlMessage { get; set; }
}
