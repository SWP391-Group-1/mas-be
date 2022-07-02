using AutoMapper;
using MAS.Core.Dtos.Incoming.Slot;
using MAS.Core.Dtos.Outcoming.Slot;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Profiles;

public class SlotProfile : Profile
{
    public SlotProfile()
    {
        // src => target
        CreateMap<SlotCreateRequest, Slot>();
        CreateMap<SlotSubjectCreateRequest, SlotSubject>();

        CreateMap<Slot, SlotResponse>();
        CreateMap<Slot, SlotDetailResponse>();
        CreateMap<SlotSubject, SlotSubjectResponse>();
    }
}
