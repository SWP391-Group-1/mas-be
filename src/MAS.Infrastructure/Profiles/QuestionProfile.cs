using AutoMapper;
using MAS.Core.Dtos.Incoming.Question;
using MAS.Core.Dtos.Outcoming.Question;
using MAS.Core.Entities;

namespace MAS.Infrastructure.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            // src => target
            CreateMap<CreateQuestionRequest, Question>();
            CreateMap<AnswerQuestionRequest, Question>();

            CreateMap<Question, QuestionResponse>();
        }
    }
}
