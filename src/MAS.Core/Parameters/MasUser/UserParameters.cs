namespace MAS.Core.Parameters.MasUser
{
    public class UserParameters : QueryStringParameters
    {
        public string Search { get; set; }
        public string SubjectId { get; set; }
        public bool? IsOrderByName { get; set; }
        public bool? IsOrderByRating { get; set; }

        public bool? IsMentor { get; set; }
    }
}
