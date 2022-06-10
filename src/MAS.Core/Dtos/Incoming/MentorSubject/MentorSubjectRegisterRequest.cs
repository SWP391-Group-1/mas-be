using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.MentorSubject;

public class MentorSubjectRegisterRequest : BaseCreateRequest
{
    [Required]
    [MaxLength(100)]
    public string SubjectId { get; set; }

    [Required]
    [MaxLength(500)]
    public string BriefInfo { get; set; }
}
