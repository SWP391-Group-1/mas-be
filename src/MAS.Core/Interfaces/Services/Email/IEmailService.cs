using MAS.Core.Dtos.Incoming.Email;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
