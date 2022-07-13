using MAS.Core.Dtos.Outcoming.MasUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAS.Core.Dtos.Outcoming.Appointment
{
    public class SlotInAppointmentResponse
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int NumOfAppointments { get; set; }
        public string MentorId { get; set; }
        public UserGetBasicInfoResponse Mentor { get; set; }
        public bool? IsPassed { get; set; }
        public bool IsActive { get; set; }
    }
}