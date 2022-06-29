using System;

namespace MAS.Core.Dtos.Outcoming.Slot;

public class SlotResponse
{
    public string Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public bool? IsPassed { get; set; }
    public bool IsActive { get; set; }
}
