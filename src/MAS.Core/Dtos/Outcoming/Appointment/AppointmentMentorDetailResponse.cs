using MAS.Core.Dtos.Outcoming.MasUser;
using MAS.Core.Dtos.Outcoming.Slot;
using System;
using System.Collections.Generic;

namespace MAS.Core.Dtos.Outcoming.Appointment;

public class AppointmentMentorDetailResponse
{
    public string Id { get; set; }
    public string CreatorId { get; set; }
    public UserGetBasicInfoResponse Creator { get; set; }

    public string SlotId { get; set; }
    public SlotDetailResponse Slot { get; set; }

    public string BriefProblem { get; set; }

    public ICollection<QuestionAppointmentResponse> Questions { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public string MentorDescription { get; set; }

    public bool? IsApprove { get; set; }
    public bool IsActive { get; set; }
    public bool? IsPassed { get; set; }
}
