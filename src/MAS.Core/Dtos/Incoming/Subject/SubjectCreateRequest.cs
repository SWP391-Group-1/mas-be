using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Subject
{
    public class SubjectCreateRequest : BaseCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string MajorId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
