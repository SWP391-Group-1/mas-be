using AutoMapper;
using MAS.Core.Dtos.Incoming.Major;
using MAS.Core.Dtos.Outcoming.Major;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Profiles
{
    public class MajorProfile : Profile
    {
        public MajorProfile()
        {
            //create, update
            CreateMap<MajorCreateRequest, Major>();
            CreateMap<MajorUpdateRequest, Major>();

            //response
            CreateMap<Major, MajorResponse>();
        }
    }
}
