using MAS.Core.Entities.Abtraction;

namespace MAS.Core.Entities
{
    public class Rating : BaseEntity
    {
        public string AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public string CreatorId { get; set; }        

        public string MentorId { get; set; }
        //public MasUser Mentor { get; set; }

        public int Vote { get; set; }
        public string Comment { get; set; }

        public bool? IsApprove { get; set; }
        public bool IsActive { get; set; }
    }
}
