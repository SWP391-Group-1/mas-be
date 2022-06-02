using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.MasUser
{
    public class MentorRequest
    {
        [Required]
        public bool IsMentor { get; set; }
    }
}
