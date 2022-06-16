using System;

namespace MAS.Core.Parameters.Slot;

public class SlotParameters : QueryStringParameters
{
    public string MentorId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool? IsAsc { get; set; }
    public bool? IsActive { get; set; }
}
