using MAS.Core.Entities.Abtraction;

namespace MAS.Core.Entities;

public class SlotSubject : BaseEntity
{
    public string SlotId { get; set; }
    public Slot Slot { get; set; }

    public string SubjectId { get; set; }
    public Subject Subject { get; set; }

    public string Description { get; set; }
}
