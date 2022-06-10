using MAS.Core.Dtos.Incoming.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Appointment;

public class AppointmentCreateRequest : BaseCreateRequest
{
    [Required]
    [MaxLength(100)]
    public string SlotId { get; set; }
    public ICollection<AppointmentSubjectCreateRequest> AppointmentSubjects { get; set; }

}
