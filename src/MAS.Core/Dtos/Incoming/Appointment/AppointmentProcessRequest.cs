using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Appointment
{
    public class AppointmentProcessRequest : BaseUpdateRequest
    {
        [Required]
        public bool IsApprove { get; set; }

        [MaxLength(1000)]
        public string MentorDescription { get; set; }
    }
}