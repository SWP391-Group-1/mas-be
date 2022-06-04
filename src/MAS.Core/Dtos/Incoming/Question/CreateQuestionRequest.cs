using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Question
{
    public class CreateQuestionRequest : BaseCreateRequest
    {
        [Required]
        public string AppointmentId { get; set; }
        [Required]
        public string CreatorId { get; set; }
        [Required]
        public string QuestionContent { get; set; }
    }
}
