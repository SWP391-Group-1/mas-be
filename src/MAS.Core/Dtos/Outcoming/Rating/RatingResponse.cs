namespace MAS.Core.Dtos.Outcoming.Rating
{
    public class RatingResponse
    {
        public string Id { get; set; }
        public string AppointmentId { get; set; }

        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorMail { get; set; }

        public string MentorId { get; set; }

        public int Vote { get; set; }
        public string Comment { get; set; }

        public bool? IsApprove { get; set; }
        public bool IsActive { get; set; }
    }
}
