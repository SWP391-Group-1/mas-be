using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Outcoming.Account;
using MAS.Core.Interfaces.Repositories.Account;
using MAS.Core.Interfaces.Services.Account;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MAS.Core.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ILogger<AccountService> logger;

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Login with Admin account service.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthenResult> LoginAdminAsync(LoginAdminRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await accountRepository.LoginAdminAsync(request);
            }
            catch (Exception ex) {
                logger.LogError($"Error while trying to call LoginAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        /// <summary>
        /// Login with Google account service.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthenResult> LoginWithGoogleAsync(GoogleLoginRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await accountRepository.LoginWithGoogleAsync(request);
            }
            catch (Exception ex) {
                logger.LogError($"Error while trying to call LoginByGoogleAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        /// <summary>
        /// Register an admin account service.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthenResult> RegisterAdminAsync(RegisterAdminRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await accountRepository.RegisterAdminAsync(request);
            }
            catch (Exception ex) {
                logger.LogError($"Error while trying to call RegisterAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
