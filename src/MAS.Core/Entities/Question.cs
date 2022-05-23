using MAS.Core.Entities.Abtraction;

namespace MAS.Core.Entities
{
    public class Question : BaseEntity
    {
        public string AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public string SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string QuestionContent { get; set; }
        public string Answer { get; set; }
    }
}
