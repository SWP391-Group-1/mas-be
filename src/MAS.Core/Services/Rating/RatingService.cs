using MAS.Core.Dtos.Incoming.Rating;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Rating;
using MAS.Core.Interfaces.Repositories.Rating;
using MAS.Core.Interfaces.Services.Rating;
using MAS.Core.Parameters.Rating;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Services.Rating;
public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly ILogger<RatingService> _logger;

    public RatingService(
        IRatingRepository ratingRepository,
        ILogger<RatingService> logger)
    {
        _ratingRepository = ratingRepository;
        _logger = logger;
    }
    public async Task<Result<bool>> CreateRatingFeedbackAsync(ClaimsPrincipal principal, string appointmentId, CreateRatingRequest request)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (String.IsNullOrEmpty(appointmentId)) {
                throw new ArgumentNullException(nameof(appointmentId));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _ratingRepository.CreateRatingFeedbackAsync(principal, appointmentId, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call CreateRatingFeedbackAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<RatingResponse>> GetAllRatingsAsync(string mentorId, RatingParameters param)
    {
        try {
            if (String.IsNullOrEmpty(mentorId)) {
                throw new ArgumentNullException(nameof(mentorId));
            }
            return await _ratingRepository.GetAllRatingsAsync(mentorId, param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllRatingsAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<RatingResponse>> GetAllRatingsForAdminAsync(RatingParametersAdmin param)
    {
        try {
            return await _ratingRepository.GetAllRatingsForAdminAsync(param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllViolateRatingsForAdminAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<RatingResponse>> GetRatingByIdAsync(string ratingId)
    {
        try {
            if (String.IsNullOrEmpty(ratingId)) {
                throw new ArgumentNullException(nameof(ratingId));
            }
            return await _ratingRepository.GetRatingByIdAsync(ratingId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetRatingByIdAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> ProcessViolateRatingAsync(string ratingId, ProcessRatingRequest request)
    {
        try {
            if (String.IsNullOrEmpty(ratingId)) {
                throw new ArgumentNullException(nameof(ratingId));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _ratingRepository.ProcessRatingAsync(ratingId, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call ProcessViolateRatingAsync in service class, Error Message: {ex}.");
            throw;
        }
    }
}
