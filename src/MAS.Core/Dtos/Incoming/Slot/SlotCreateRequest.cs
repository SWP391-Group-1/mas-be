using MAS.Core.Dtos.Incoming.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Slot;

public class SlotCreateRequest : BaseCreateRequest
{
    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime FinishTime { get; set; }

    public ICollection<SlotSubjectCreateRequest> SlotSubjects { get; set; }
}
