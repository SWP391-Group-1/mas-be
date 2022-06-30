using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.MasUser;

public class IsActiveChangeRequest : BaseUpdateRequest
{
    [Required]
    public bool IsActive { get; set; }
    public string Note { get; set; }

    //public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
}
