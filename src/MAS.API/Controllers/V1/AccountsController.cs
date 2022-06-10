using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Outcoming.Account;
using MAS.Core.Interfaces.Services.Account;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1
{
    [ApiVersion("1.0")]
    public class AccountsController : BaseController
    {
        private readonly IAccountService accountService;

        public AccountsController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Login with Admin Account.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("login-admin")]
        public async Task<ActionResult<AuthenResult>> LoginAdmin(LoginAdminRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await accountService.LoginAdminAsync(request);
            if (!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Login with google
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("login-google")]
        public async Task<ActionResult<AuthenResult>> LoginWithGoogle(GoogleLoginRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await accountService.LoginWithGoogleAsync(request);
            if (!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("login-user")]
        public async Task<ActionResult<AuthenResult>> LoginUser(LoginUserRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await accountService.LoginUserAsync(request);
            if (!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Register admin account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("register-admin")]
        public async Task<ActionResult<AuthenResult>> AdminRegister(RegisterAdminRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await accountService.RegisterAdminAsync(request);
            if (!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Register user account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("register-user")]
        public async Task<ActionResult<AuthenResult>> UserRegister(RegisterUserRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await accountService.RegisterUserAsync(request);
            if (!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
