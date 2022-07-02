using MAS.Core.Entities.Abtraction;
using System;
using System.Collections.Generic;

namespace MAS.Core.Entities;

public class Slot : BaseEntity
{
    public string MentorId { get; set; }
    public MasUser Mentor { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public bool? IsPassed { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<SlotSubject> SlotSubjects { get; set; }
}
