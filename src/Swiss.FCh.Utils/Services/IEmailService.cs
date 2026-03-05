using Swiss.FCh.Utils.Models;

namespace Swiss.FCh.Utils.Services;

public interface IEmailService
{
    Task Send(Email email);
}
