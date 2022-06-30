using MAS.Core.Dtos.Incoming.Generic;

namespace MAS.Core.Dtos.Incoming.MasUser;

public class MentorRequest : BaseUpdateRequest
{
    public bool? IsMentor { get; set; }
}
