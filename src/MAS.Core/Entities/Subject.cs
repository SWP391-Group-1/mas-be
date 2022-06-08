using MAS.Core.Entities.Abtraction;
using System.Collections.Generic;

namespace MAS.Core.Entities
{
    public class Subject : BaseEntity
    {
        public string MajorId { get; set; }
        public Major Major { get; set; }

        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<MentorSubject> MentorSubjects { get; set; }
    }
}
