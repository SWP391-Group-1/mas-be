namespace MAS.Core.Parameters.Appointment
{
    public class AppointmentMentorParameters : QueryStringParameters
    {
        public bool? IsNew { get; set; }
        public string SlotId { get; set; }
    }
}