using AutoMapper;
using MAS.Core.Dtos.Incoming.Subject;
using MAS.Core.Dtos.Outcoming.Subject;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Profiles
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            // src => target
            CreateMap<SubjectCreateRequest, Subject>();
            CreateMap<SubjectUpdateRequest, Subject>();

            CreateMap<Subject, SubjectResponse>();
            CreateMap<Subject, SubjectDetailResponse>();
        }
    }
}
