using MAS.Core.Entities.Abtraction;
using System;
using System.Collections.Generic;

namespace MAS.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public string CreatorId { get; set; }
        //public MasUser Creator { get; set; }

        public string MentorId { get; set; }

        public string SlotId { get; set; }
        public Slot Slot { get; set; }

        public ICollection<AppointmentSubject> AppointmentSubjects { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Rating> Ratings { get; set; }

        public bool IsApprove { get; set; }

        public DateTime? StartTime { get; set; }

        public string MentorDescription { get; set; }
        public DateTime? FinishTime { get; set; }
    }
}
