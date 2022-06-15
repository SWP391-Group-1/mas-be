using AutoMapper;
using MAS.Core.Dtos.Incoming.Rating;
using MAS.Core.Dtos.Outcoming.Rating;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Profiles;
public class RatingProfile : Profile
{
    public RatingProfile()
    {
        //create, update
        CreateMap<CreateRatingRequest, Rating>();
        CreateMap<ProcessRatingRequest, Rating>();

        //response
        CreateMap<Rating, RatingResponse>();
        CreateMap<MasUser, UserRateResponse>();
    }

}
