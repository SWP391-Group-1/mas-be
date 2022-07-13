using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Slot;

public class SlotSubjectCreateRequest : BaseCreateRequest
{
    [Required]
    [MaxLength(100)]
    public string SubjectId { get; set; }

    [Required]
    [MaxLength(500)]
    // [MinLength(50)]
    public string Description { get; set; }
}
