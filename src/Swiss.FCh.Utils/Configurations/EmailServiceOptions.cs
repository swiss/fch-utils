using System.ComponentModel.DataAnnotations;

namespace Swiss.FCh.Utils.Configurations;

/// <summary>
/// This class defines the options that are required to use the <see cref="Swiss.FCh.Utils.Services.IEmailService"/>.
/// </summary>
public class EmailServiceOptions
{
    /// <summary>
    /// This key defines the name of the section (e.g. in appsettings.json) where the options for the <see cref="Swiss.FCh.Utils.Services.IEmailService"/> must be registered.
    /// </summary>
    public const string SectionKey = "EmailService";

    /// <summary>
    /// URL of the SMTP server.
    /// </summary>
    [Required]
    public required string Host { get; init; }

    /// <summary>
    /// Port of the SMTP server.
    /// </summary>
    [Required]
    public required int Port { get; init; }
}
