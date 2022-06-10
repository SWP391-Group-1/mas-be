using MAS.Core.Dtos.Incoming.Email;
using MAS.Core.Interfaces.Repositories.Email;
using MAS.Core.Interfaces.Services.Email;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MAS.Core.Services.Email;

public class EmailService : IEmailService
{
    private readonly IEmailRepository _mailRepository;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IEmailRepository mailRepository, ILogger<EmailService> logger)
    {
        _mailRepository = mailRepository;
        _logger = logger;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        try {
            if (mailRequest is null) {
                throw new ArgumentNullException(nameof(mailRequest));
            }
            await _mailRepository.SendEmailAsync(mailRequest);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call SendEmailAsync in service class, Error Message: {ex}.");
            throw;
        }
    }
}
