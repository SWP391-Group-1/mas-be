﻿namespace MAS.Core.Dtos.Outcoming.Question;

public class QuestionResponse
{
    public string Id { get; set; }

    // public string CreatorId { get; set; }
    //public MasUser Creator { get; set; }

    public string QuestionContent { get; set; }
    public string AppointmentId { get; set; }
    public string Answer { get; set; }
    public bool IsActive { get; set; }
}
