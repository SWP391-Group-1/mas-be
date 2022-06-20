using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Rating
{
    public class CreateRatingRequest : BaseCreateRequest
    {
        [Required]
        [Range(1, 5)]
        public int Vote { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }
}
