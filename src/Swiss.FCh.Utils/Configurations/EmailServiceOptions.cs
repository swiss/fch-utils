using System.ComponentModel.DataAnnotations;

namespace Swiss.FCh.Utils.Configurations;

public class EmailServiceOptions
{
    public const string SectionKey = "EmailService";

    [Required]
    public required string Host { get; init; }
    [Required]
    public required int Port { get; init; }
}
