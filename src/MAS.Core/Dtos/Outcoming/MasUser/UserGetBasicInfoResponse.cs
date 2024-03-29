﻿using System;

namespace MAS.Core.Dtos.Outcoming.MasUser;

public class UserGetBasicInfoResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string Introduce { get; set; }
    public string MeetUrl { get; set; }
    public float Rate { get; set; }
    public int NumOfRate { get; set; }
    public bool? IsMentor { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreateDate { get; set; }




    //public ICollection<SlotResponse> Slots { get; set; }
    //public ICollection<MentorSubjectResponse> MentorSubjects { get; set; }
}
