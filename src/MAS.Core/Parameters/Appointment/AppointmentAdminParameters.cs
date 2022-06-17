namespace MAS.Core.Parameters.Appointment;

public class AppointmentAdminParameters : QueryStringParameters
{
    public bool? IsNew { get; set; }
    public string SlotId { get; set; }
    public bool? IsActive { get; set; }
}
