using MAS.Core.Dtos.Incoming.Email;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.Email;

public interface IEmailRepository
{
    Task SendEmailAsync(MailRequest mailRequest);
}
