using MAS.Core.Dtos.Incoming.Question;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Question;
using MAS.Core.Interfaces.Repositories.Question;
using MAS.Core.Interfaces.Services.Question;
using MAS.Core.Parameters.Question;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Services.Question;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ILogger<QuestionService> _logger;

    public QuestionService(
        IQuestionRepository questionRepository,
        ILogger<QuestionService> logger)
    {
        _questionRepository = questionRepository;
        _logger = logger;
    }

    public async Task<Result<QuestionResponse>> AnswerQuestion(ClaimsPrincipal principal, string questionId, AnswerQuestionRequest request)
    {
        try {
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (questionId is null) {
                throw new ArgumentNullException(nameof(questionId));
            }
            return await _questionRepository.AnswerQuestionAsync(principal, questionId, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call AnswerQuestion in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<QuestionResponse>> CreateQuestionAsync(ClaimsPrincipal principal, CreateQuestionRequest request)
    {
        try {
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }

            return await _questionRepository.CreateQuestionAsync(principal, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call CreateQuestionAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> DeleteQuestion(ClaimsPrincipal principal, string questionId)
    {
        try {
            if (questionId is null) {
                throw new ArgumentNullException(nameof(questionId));
            }
            return await _questionRepository.DeleteQuestionAsync(principal, questionId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call DeleteQuestion in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<QuestionResponse>> GetAllQuestionAsync(string appointmentId, QuestionParameters param)
    {
        try {
            if (String.IsNullOrEmpty(appointmentId) || String.IsNullOrWhiteSpace(appointmentId)) {
                throw new ArgumentNullException(nameof(appointmentId));
            }
            return await _questionRepository.GetAllQuestionAsync(appointmentId, param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllQuestionAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<QuestionResponse>> GetAllQuestionOfSlotAsync(string slotId, QuestionParameters param)
    {
        try {
            if (String.IsNullOrEmpty(slotId) || String.IsNullOrWhiteSpace(slotId)) {
                throw new ArgumentNullException(nameof(slotId));
            }
            return await _questionRepository.GetAllQuestionOfSlotAsync(slotId, param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllQuestionOfSlotAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<QuestionResponse>> GetQuestionById(string questionId)
    {
        try {
            if (questionId is null) {
                throw new ArgumentNullException(nameof(questionId));
            }
            return await _questionRepository.GetQuestionByIdAsync(questionId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetQuestionById in service class, Error Message: {ex}.");
            throw;
        }
    }
}
