namespace MAS.Core.Dtos.Outcoming.MentorSubject;

public class MentorSubjectResponse
{
    public string Id { get; set; }

    public string SubjectId { get; set; }
    public SubjectOfMentorResponse Subject { get; set; }

    public string BriefInfo { get; set; }
    public bool IsActive { get; set; }
}
