using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.MentorSubject;

public class MentorSubjectUpdateRequest : BaseUpdateRequest
{
    [Required]
    [MaxLength(500)]
    public string BriefInfo { get; set; }
}
