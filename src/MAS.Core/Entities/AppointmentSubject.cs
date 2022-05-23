using MAS.Core.Entities.Abtraction;

namespace MAS.Core.Entities
{
    public class AppointmentSubject : BaseEntity
    {
        public string AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public string SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string BriefProblem { get; set; }
    }
}
