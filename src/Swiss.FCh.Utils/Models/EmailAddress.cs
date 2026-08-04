namespace Swiss.FCh.Utils.Models;

/// <summary>
/// This class represents an e-mail address used with <see cref="Swiss.FCh.Utils.Services.IEmailService"/>.
/// </summary>
public class EmailAddress
{
    /// <summary>
    /// Display name of the e-mail address.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The technical e-mail address itself.
    /// </summary>
    public required string Address { get; init; }
}
