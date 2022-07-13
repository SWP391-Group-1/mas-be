using System;

namespace MAS.Core.Parameters.Slot;

public class SlotParameters : QueryStringParameters
{
    public string MentorId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool? IsAsc { get; set; }
    public bool? IsIn { get; set; }
    public bool? IsOwn { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPassed { get; set; }
}
