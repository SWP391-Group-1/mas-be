using AutoMapper;
using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Profiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<RegisterUserRequest, MasUser>();
    }
}
