using MAS.Core.Dtos.Incoming.Generic;

namespace MAS.Core.Dtos.Incoming.MasUser;

public class UserPersonalInfoUpdateRequest : BaseUpdateRequest
{
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string Introduce { get; set; }
    public string MeetUrl { get; set; }
}
