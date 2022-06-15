using MAS.Core.Dtos.Incoming.Major;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Major;
using MAS.Core.Interfaces.Repositories.Major;
using MAS.Core.Interfaces.Services.Major;
using MAS.Core.Parameters.Major;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MAS.Core.Services.Major;

public class MajorService : IMajorService
{
    private readonly IMajorRepository _majorRepository;
    private readonly ILogger<MajorService> _logger;

    public MajorService(IMajorRepository majorRepository, ILogger<MajorService> logger)
    {
        _logger = logger;
        _majorRepository = majorRepository;

    }

    public async Task<Result<MajorResponse>> CreateMajorAsync(MajorCreateRequest request)
    {
        try {
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _majorRepository.CreateMajorAsync(request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call CreateMajorAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> DeleteMajorAsync(string majorId)
    {
        try {
            if (String.IsNullOrEmpty(majorId) || String.IsNullOrWhiteSpace(majorId)) {
                throw new ArgumentNullException(nameof(majorId));
            }
            return await _majorRepository.DeleteMajorAsync(majorId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call DeleteMajorAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<MajorResponse>> GetAllMajorsAsync(MajorParameters param)
    {
        try {
            return await _majorRepository.GetAllMajorsAsync(param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllMajorsAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<MajorResponse>> GetMajorByIdAsync(string majorId)
    {
        try {
            if (String.IsNullOrEmpty(majorId) || String.IsNullOrWhiteSpace(majorId)) {
                throw new ArgumentNullException(nameof(majorId));
            }
            return await _majorRepository.GetMajorByIdAsync(majorId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetMajorByIdAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> UpdateMajorAsync(string majorId, MajorUpdateRequest request)
    {
        try {
            if (String.IsNullOrEmpty(majorId) || String.IsNullOrWhiteSpace(majorId)) {
                throw new ArgumentNullException(nameof(majorId));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _majorRepository.UpdateMajorAsync(majorId, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call UpdateMajorAsync in service class, Error Message: {ex}.");
            throw;
        }
    }
}
