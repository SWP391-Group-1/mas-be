using MAS.Core.Dtos.Outcoming.Subject;

namespace MAS.Core.Dtos.Outcoming.Appointment;

public class AppointmentSubjectResponse
{
    public string Id { get; set; }
    public string SubjectId { get; set; }
    public SubjectResponse Subject { get; set; }

    public string BriefProblem { get; set; }
    public bool IsActive { get; set; }
}
