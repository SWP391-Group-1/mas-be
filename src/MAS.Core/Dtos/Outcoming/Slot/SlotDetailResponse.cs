using MAS.Core.Dtos.Outcoming.MasUser;
using System;

namespace MAS.Core.Dtos.Outcoming.Slot;

public class SlotDetailResponse
{
    public string Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }

    public string MentorId { get; set; }
    public UserGetBasicInfoResponse Mentor { get; set; }
    public bool IsActive { get; set; }
}
