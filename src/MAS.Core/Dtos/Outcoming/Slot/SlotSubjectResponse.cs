using MAS.Core.Dtos.Outcoming.Subject;

namespace MAS.Core.Dtos.Outcoming.Slot;

public class SlotSubjectResponse
{
    public string Id { get; set; }
    public string SubjectId { get; set; }
    public SubjectResponse Subject { get; set; }

    public string Description { get; set; }
    public bool IsActive { get; set; }
}
