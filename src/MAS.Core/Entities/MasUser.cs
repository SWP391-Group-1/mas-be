using MAS.Core.Entities.Abtraction;
using System.Collections.Generic;

namespace MAS.Core.Entities
{
    public class MasUser : BaseEntity
    {
        public string IdentityId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string MeetUrl { get; set; }
        public bool? IsMentor { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Slot> Slots { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MentorSubject> MentorSubjects { get; set; }
    }
}
