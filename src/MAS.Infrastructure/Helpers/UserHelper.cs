using MAS.Core.Dtos.Outcoming.MasUser;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Helpers
{
    public static class UserHelper
    {
        public static UserGetBasicInfoResponse PopulateUser(MasUser user)
        {
            return new UserGetBasicInfoResponse {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Avatar = user.Avatar,
                Introduce = user.Introduce,
                MeetUrl = user.MeetUrl,
                Rate = user.Rate,
                NumOfRate = user.NumOfRate,
                IsMentor = user.IsMentor,
                IsActive = user.IsActive,
                CreateDate = user.CreateDate
            };
        }
    }
}