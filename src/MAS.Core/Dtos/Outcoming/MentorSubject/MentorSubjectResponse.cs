using MAS.Core.Dtos.Outcoming.Subject;

namespace MAS.Core.Dtos.Outcoming.MentorSubject;

public class MentorSubjectResponse
{
    public string Id { get; set; }

    public string SubjectId { get; set; }
    public SubjectDetailResponse Subject { get; set; }

    public string BriefInfo { get; set; }
    public bool IsActive { get; set; }
}
