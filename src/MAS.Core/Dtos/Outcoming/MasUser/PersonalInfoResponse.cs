namespace MAS.Core.Dtos.Outcoming.MasUser;

public class PersonalInfoResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string Introduce { get; set; }
    public string MeetUrl { get; set; }
    public float Rate { get; set; }
    public int NumOfRate { get; set; }
    public int NumOfAppointment { get; set; }
    public bool? IsMentor { get; set; }
    public bool IsActive { get; set; } = true;

    //public ICollection<SlotResponse> Slots { get; set; }
    //public ICollection<MentorSubjectResponse> MentorSubjects { get; set; }
    //public ICollection<AppointmentResponse> Appointments { get; set; }
    //public ICollection<RatingResponse> Ratings { get; set; }
}
