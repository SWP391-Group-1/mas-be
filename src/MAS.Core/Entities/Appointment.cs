using System.Collections.Generic;

namespace MAS.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public string StudentId { get; set; }
        public MasUser Student { get; set; }

        public string MentorId { get; set; }
        //public MasUser Mentor { get; set; }

        public string SlotId { get; set; }
        public Slot Slot { get; set; }

        public ICollection<AppointmentSubject> AppointmentSubjects { get; set; }
        public ICollection<Question> Questions { get; set; }

        public bool IsApproved { get; set; }

    }
}
