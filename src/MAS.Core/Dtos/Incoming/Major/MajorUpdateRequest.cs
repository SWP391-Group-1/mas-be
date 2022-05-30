using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Major
{
    public class MajorUpdateRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
