using AutoMapper;
using MAS.Core.Dtos.Incoming.Appointment;
using MAS.Core.Dtos.Outcoming.Appointment;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Profiles;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        // src => target
        CreateMap<AppointmentCreateRequest, Appointment>();
        CreateMap<AppointmentProcessRequest, Appointment>();
        CreateMap<AppointmentUpdateRequest, Appointment>();
        CreateMap<AppointmentSubjectCreateRequest, AppointmentSubject>(); 
        
        CreateMap<Appointment, AppointmentUserResponse>();
        CreateMap<Appointment, AppointmentUserDetailResponse>();
        CreateMap<Appointment, AppointmentMentorResponse>();
        CreateMap<Appointment, AppointmentMentorDetailResponse>();
        CreateMap<Appointment, AppointmentAdminResponse>();
        CreateMap<Appointment, AppointmentAdminDetailResponse>();
    }
}
