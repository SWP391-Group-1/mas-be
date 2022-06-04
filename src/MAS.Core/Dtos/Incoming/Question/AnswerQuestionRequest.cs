using MAS.Core.Dtos.Incoming.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Question
{
    public class AnswerQuestionRequest : BaseUpdateRequest
    {
        [Required]
        public string Answer { get; set; }
    }
}
