using AutoMapper;
using MAS.Core.Dtos.Incoming.MentorSubject;
using MAS.Core.Dtos.Outcoming.MentorSubject;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Profiles;

public class MentorSubjectProfile : Profile
{
    public MentorSubjectProfile()
    {
        // src => target
        CreateMap<MentorSubjectRegisterRequest, MentorSubject>();
        CreateMap<MentorSubjectUpdateRequest, MentorSubject>();

        CreateMap<MentorSubject, MentorSubjectResponse>();

        CreateMap<Subject, SubjectOfMentorResponse>();
    }
}
