using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Appointment;

public class AppointmentCreateRequest : BaseCreateRequest
{
    [Required]
    [MaxLength(100)]
    public string SlotId { get; set; }

    [Required]
    [MaxLength(500)]
    public string BriefProblem { get; set; }
}
