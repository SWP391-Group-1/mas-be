using System;

namespace MAS.Core.Entities
{
    public class Slot : BaseEntity
    {
        public string MentorId { get; set; }
        public MasUser Mentor { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
    }
}
