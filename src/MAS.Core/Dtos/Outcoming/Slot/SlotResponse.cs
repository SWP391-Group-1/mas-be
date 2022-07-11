using MAS.Core.Dtos.Outcoming.MasUser;
using System;
using System.Collections.Generic;

namespace MAS.Core.Dtos.Outcoming.Slot;

public class SlotResponse
{
    public string Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public int NumOfAppointments { get; set; }

    public ICollection<SlotSubjectResponse> SlotSubjects { get; set; }
    public string MentorId { get; set; }
    public UserGetBasicInfoResponse Mentor { get; set; }
    public bool IsIn { get; set; }
    public bool IsOwn { get; set; }
    public bool? IsPassed { get; set; }
    public bool IsActive { get; set; }
}
