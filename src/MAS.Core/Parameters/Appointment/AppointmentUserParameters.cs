namespace MAS.Core.Parameters.Appointment;

public class AppointmentUserParameters : QueryStringParameters
{
    public bool? IsNew { get; set; }
    public string SlotId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsApprove { get; set; }
}
