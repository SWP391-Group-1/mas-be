using MAS.Core.Entities.Abtraction;

namespace MAS.Core.Entities
{
    public class MentorSubject : BaseEntity
    {
        public string MentorId { get; set; }
        public MasUser Mentor { get; set; }

        public string SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string BriefInfo { get; set; }
    }
}
