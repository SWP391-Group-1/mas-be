namespace MAS.Core.Entities
{
    public class Slot : BaseEntity
    {
        public string MentorId { get; set; }
        public MasUser Mentor { get; set; }
        public int DateInWeek { get; set; }
        public long StartTime { get; set; }
        public long FinishTime { get; set; }
    }
}
