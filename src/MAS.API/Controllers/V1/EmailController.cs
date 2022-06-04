using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.Email;
using MAS.Core.Interfaces.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1
{
    [ApiVersion("1.0")]
    public class EmailController : BaseController
    {
        private readonly IEmailService _mailService;
        public EmailController(IEmailService mailService)
        {
            _mailService = mailService;
        }

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpPost("send")]
        [Authorize(Roles = RoleConstants.User + "," + RoleConstants.Admin)]
        public async Task<ActionResult> Send(MailRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            await _mailService.SendEmailAsync(request);
            return Ok();
        }
    }
}