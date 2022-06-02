using AutoMapper;
using MAS.Core.Dtos.Incoming.MasUser;
using MAS.Core.Dtos.Outcoming.MasUser;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Profiles
{
    public class MasUserProfile : Profile
    {
        public MasUserProfile()
        {
            CreateMap<MasUser, PersonalInfoResponse>();

            CreateMap<UserPersonalInfoUpdateRequest, MasUser>();
            CreateMap<IsActiveChangeRequest, MasUser>();
            CreateMap<MentorRequest, MasUser>();

            CreateMap<MasUser, UserGetBasicInfoResponse>();
            CreateMap<MasUser, UserSearchResponse>();
            CreateMap<MasUser, UserGetByAdminResponse>();
        }
    }
}
