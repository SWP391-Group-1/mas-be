using System;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.MasUser
{
    public class IsActiveChangeRequest
    {
        [Required]
        public bool IsActive { get; set; }
        public string Note { get; set; }

        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}
