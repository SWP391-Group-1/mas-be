using MAS.Core.Dtos.Incoming.Question;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Question;
using MAS.Core.Parameters.Question;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.Question;

public interface IQuestionRepository
{
    Task<Result<QuestionResponse>> CreateQuestionAsync(ClaimsPrincipal principal, CreateQuestionRequest request);
    Task<PagedResult<QuestionResponse>> GetAllQuestionAsync(string appointmentId, QuestionParameters param);
    Task<Result<QuestionResponse>> GetQuestionById(string questionId);
    Task<Result<QuestionResponse>> AnswerQuestion(ClaimsPrincipal principal, string questionId, AnswerQuestionRequest request);
    Task<Result<bool>> DeleteQuestion(ClaimsPrincipal principal, string questionId);
}
