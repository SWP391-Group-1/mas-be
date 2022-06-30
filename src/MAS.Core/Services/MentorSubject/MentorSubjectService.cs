using MAS.Core.Dtos.Incoming.MentorSubject;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.MentorSubject;
using MAS.Core.Interfaces.Repositories.MentorSubject;
using MAS.Core.Interfaces.Services.MentorSubject;
using MAS.Core.Parameters.MentorSubject;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Services.MentorSubject;

public class MentorSubjectService : IMentorSubjectService
{
    private readonly IMentorSubjectRepository _mentorSubjectRepository;
    private readonly ILogger<MentorSubjectService> _logger;

    public MentorSubjectService(
        IMentorSubjectRepository mentorSubjectRepository,
        ILogger<MentorSubjectService> logger)
    {
        _mentorSubjectRepository = mentorSubjectRepository;
        _logger = logger;
    }
    public async Task<Result<bool>> DeleteSubjectOfMentorAsync(
        ClaimsPrincipal principal,
        string subjectOfMentorId)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (String.IsNullOrEmpty(subjectOfMentorId) || String.IsNullOrWhiteSpace(subjectOfMentorId)) {
                throw new ArgumentNullException(nameof(subjectOfMentorId));
            }
            return await _mentorSubjectRepository.DeleteSubjectOfMentorAsync(principal, subjectOfMentorId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call DeleteSubjectOfMentorAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<MentorSubjectResponse>> GetAllSubjectsOfMentorAsync(
        string mentorId,
        MentorSubjectParameters param)
    {
        try {
            if (String.IsNullOrEmpty(mentorId) || String.IsNullOrWhiteSpace(mentorId)) {
                throw new ArgumentNullException(nameof(mentorId));
            }
            return await _mentorSubjectRepository.GetAllSubjectsOfMentorAsync(mentorId, param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllsSubjectOfMentorAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> RegisterSubjectAsync(
        ClaimsPrincipal principal,
        MentorSubjectRegisterRequest request)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _mentorSubjectRepository.RegisterSubjectAsync(principal, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call RegisterSubjectAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> UpdateSubjectOfMentorAsync(
        ClaimsPrincipal principal,
        string subjectOfMentorId,
        MentorSubjectUpdateRequest request)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (String.IsNullOrEmpty(subjectOfMentorId) || String.IsNullOrWhiteSpace(subjectOfMentorId)) {
                throw new ArgumentNullException(nameof(subjectOfMentorId));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _mentorSubjectRepository.UpdateSubjectOfMentorAsync(principal, subjectOfMentorId, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call UpdateSubjectOfMentorAsync in service class, Error Message: {ex}.");
            throw;
        }
    }
}
