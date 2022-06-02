namespace MAS.Core.Parameters.MasUser
{
    public class AdminUserParameters : QueryStringParameters
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsNew { get; set; }
    }
}
