using MAS.Core.Dtos.Incoming.Rating;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Rating;
using MAS.Core.Parameters.Rating;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Services.Rating;

public interface IRatingService
{
    Task<Result<bool>> CreateRatingFeedbackAsync(ClaimsPrincipal principal, string appointmentId, CreateRatingRequest request);
    Task<PagedResult<RatingResponse>> GetAllRatingsAsync(string mentorId, RatingParameters param);
    Task<Result<RatingResponse>> GetRatingByIdAsync(string ratingId);
    Task<Result<RatingResponse>> GetRatingByAppointmentIdAsync(string appointmentId);
    Task<PagedResult<RatingResponse>> GetAllRatingsForAdminAsync(RatingParametersAdmin param);
    Task<Result<bool>> ProcessViolateRatingAsync(string ratingId, ProcessRatingRequest request);
}
