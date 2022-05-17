using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Outcoming.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.Account
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Login user with Google Account.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AuthenResult> LoginWithGoogleAsync(GoogleLoginRequest request);

        /// <summary>
        /// Register admin account.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AuthenResult> RegisterAdminAsync(RegisterAdminRequest request);

        /// <summary>
        /// Login with Admin account.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AuthenResult> LoginAdminAsync(LoginAdminRequest request);
    }
}
