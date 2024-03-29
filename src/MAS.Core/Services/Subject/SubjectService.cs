using MAS.Core.Dtos.Incoming.Subject;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Subject;
using MAS.Core.Interfaces.Repositories.Subject;
using MAS.Core.Interfaces.Services.Subject;
using MAS.Core.Parameters.Subject;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MAS.Core.Services.Subject;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly ILogger<SubjectService> _logger;

    public SubjectService(ISubjectRepository subjectRepository, ILogger<SubjectService> logger)
    {
        _logger = logger;
        _subjectRepository = subjectRepository;

    }

    public async Task<Result<SubjectResponse>> CreateSubjectAsync(SubjectCreateRequest request)
    {
        try {
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _subjectRepository.CreateSubjectAsync(request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call CreateSubjectAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> DeleteSubjectAsync(string subjectId)
    {
        try {
            if (String.IsNullOrEmpty(subjectId) || String.IsNullOrWhiteSpace(subjectId)) {
                throw new ArgumentNullException(nameof(subjectId));
            }
            return await _subjectRepository.DeleteSubjectAsync(subjectId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call DeleteSubjectAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<SubjectResponse>> GetAllSubjectsAsync(SubjectParameters param)
    {
        try {
            return await _subjectRepository.GetAllSubjectsAsync(param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllSubjectsAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<SubjectDetailResponse>> GetSubjectByIdAsync(string subjectId)
    {
        try {
            if (String.IsNullOrEmpty(subjectId) || String.IsNullOrWhiteSpace(subjectId)) {
                throw new ArgumentNullException(nameof(subjectId));
            }
            return await _subjectRepository.GetSubjectByIdAsync(subjectId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call DeleteSubjectAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> UpdateSubjectAsync(string subjectId, SubjectUpdateRequest request)
    {
        try {
            if (String.IsNullOrEmpty(subjectId) || String.IsNullOrWhiteSpace(subjectId)) {
                throw new ArgumentNullException(nameof(subjectId));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _subjectRepository.UpdateSubjectAsync(subjectId, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call UpdateSubjectAsync in service class, Error Message: {ex}.");
            throw;
        }
    }
}
