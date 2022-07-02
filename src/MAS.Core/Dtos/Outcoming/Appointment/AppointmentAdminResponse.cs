using MAS.Core.Dtos.Outcoming.MasUser;
using MAS.Core.Dtos.Outcoming.Slot;
using System;

namespace MAS.Core.Dtos.Outcoming.Appointment;

public interface AppointmentAdminResponse
{
    public string Id { get; set; }
    public string CreatorId { get; set; }
    public UserGetBasicInfoResponse Creator { get; set; }

    public string MentorId { get; set; }
    public UserGetBasicInfoResponse Mentor { get; set; }

    public string SlotId { get; set; }
    public SlotResponse Slot { get; set; }

    public string BriefProblem { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }

    public bool? IsApprove { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsActive { get; set; }
    public bool? IsPassed { get; set; }
}
