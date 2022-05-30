using MAS.Core.Entities.Abtraction;

namespace MAS.Core.Entities
{
    public class Question : BaseEntity
    {
        public string AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public string CreatorId { get; set; }
        //public MasUser Creator { get; set; }

        public string QuestionContent { get; set; }
        public string Answer { get; set; }
    }
}
