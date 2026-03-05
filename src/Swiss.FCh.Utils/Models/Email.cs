namespace Swiss.FCh.Utils.Models;

public class Email
{
    public required EmailAddress From { get; set; }
    public required IEnumerable<EmailAddress> To { get; set; }
    public required string Subject { get; set; }
    public string? TextMessage { get; set; }
    public string? HtmlMessage { get; set; }
}
