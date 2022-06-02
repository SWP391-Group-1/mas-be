using MAS.Core.Dtos.Incoming.MasUser;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.MasUser;
using MAS.Core.Interfaces.Repositories.MasUser;
using MAS.Core.Interfaces.Services.MasUser;
using MAS.Core.Parameters.MasUser;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Services.MasUser
{
    public class MasUserService : IMasUserService
    {
        private readonly IMasUserRepository _masUserRepository;
        private readonly ILogger<MasUserService> _logger;

        public MasUserService(IMasUserRepository masUserRepository, ILogger<MasUserService> logger)
        {
            _logger = logger;
            _masUserRepository = masUserRepository;

        }

        public async Task<Result<bool>> ChangeIsActiveUserForAdminAsync(string userId, IsActiveChangeRequest request)
        {
            try {
                if (String.IsNullOrEmpty(userId) || String.IsNullOrWhiteSpace(userId)) {
                    throw new ArgumentNullException(nameof(userId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _masUserRepository.ChangeIsActiveUserForAdminAsync(userId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ChangeIsActiveUserForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<UserSearchResponse>> GetAllMentorsAsync(ClaimsPrincipal principal, UserParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _masUserRepository.GetAllMentorsAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllUsersAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<UserGetByAdminResponse>> GetAllUsersForAdminAsync(AdminUserParameters param)
        {
            try {
                return await _masUserRepository.GetAllUsersForAdminAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllUsersForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<PersonalInfoResponse>> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _masUserRepository.GetPersonalInfoByIdentityIdAsync(principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetPersonalInfoByIdentityIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<UserGetBasicInfoResponse>> GetUserBasicInfoByIdAsync(string userId)
        {
            try {
                if (String.IsNullOrEmpty(userId) || String.IsNullOrWhiteSpace(userId)) {
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _masUserRepository.GetUserBasicInfoByIdAsync(userId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetUserBasicInfoByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> SendMentorRequest(ClaimsPrincipal principal, MentorRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> UpdatePersonalInfoAsync(ClaimsPrincipal principal, UserPersonalInfoUpdateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _masUserRepository.UpdatePersonalInfoAsync(principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdatePersonalInfoAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
