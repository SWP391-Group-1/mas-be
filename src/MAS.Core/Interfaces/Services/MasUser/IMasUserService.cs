using MAS.Core.Dtos.Incoming.MasUser;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.MasUser;
using MAS.Core.Parameters.MasUser;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Services.MasUser
{
    public interface IMasUserService
    {
        Task<Result<PersonalInfoResponse>> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal);
        Task<Result<bool>> UpdatePersonalInfoAsync(ClaimsPrincipal principal, UserPersonalInfoUpdateRequest request);
        Task<Result<bool>> SendMentorRequest(ClaimsPrincipal principal);
        Task<Result<bool>> AcceptRequest(string userId, MentorRequest request);

        Task<Result<UserGetBasicInfoResponse>> GetUserBasicInfoByIdAsync(string userId);
        Task<PagedResult<UserSearchResponse>> GetAllMentorsAsync(ClaimsPrincipal principal, UserParameters param);

        Task<PagedResult<UserGetByAdminResponse>> GetAllUsersForAdminAsync(AdminUserParameters param);
        Task<Result<bool>> ChangeIsActiveUserForAdminAsync(string userId, IsActiveChangeRequest request);
    }
}
