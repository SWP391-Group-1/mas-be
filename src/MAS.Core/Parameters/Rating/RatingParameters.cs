namespace MAS.Core.Parameters.Rating
{
    public class RatingParameters : QueryStringParameters
    {
        public float Vote { get; set; } = 0;
        public bool? IsNew { get; set; }
    }
}
