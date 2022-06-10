namespace MAS.Core.Parameters.Subject;

public class SubjectParameters : QueryStringParameters
{
    public string SearchString { get; set; }
    public string MajorId { get; set; }
    public bool? SortAsc { get; set; }
}
