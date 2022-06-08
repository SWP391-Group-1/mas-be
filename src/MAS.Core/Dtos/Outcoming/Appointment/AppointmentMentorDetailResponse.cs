using MAS.Core.Dtos.Outcoming.MasUser;
using MAS.Core.Dtos.Outcoming.Slot;
using System.Collections.Generic;

namespace MAS.Core.Dtos.Outcoming.Appointment
{
    public class AppointmentMentorDetailResponse
    {
        public string CreatorId { get; set; }
        public UserGetBasicInfoResponse Creator { get; set; }

        public string SlotId { get; set; }
        public SlotResponse Slot { get; set; }

        public ICollection<AppointmentSubjectResponse> AppointmentSubjects { get; set; }
        public ICollection<QuestionAppointmentResponse> Questions { get; set; }

        public bool? IsApprove { get; set; }
    }
}