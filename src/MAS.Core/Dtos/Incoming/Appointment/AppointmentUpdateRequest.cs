using MAS.Core.Dtos.Incoming.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Appointment;

public class AppointmentUpdateRequest : BaseUpdateRequest
{
    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime FinishTime { get; set; }

    [Required]
    public bool IsPassed { get; set; }

    [Required]
    [MaxLength(1000)]
    public string MentorDescription { get; set; }
}
