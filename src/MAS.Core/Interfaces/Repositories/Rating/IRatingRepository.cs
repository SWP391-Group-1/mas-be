using MAS.Core.Dtos.Incoming.Rating;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Rating;
using MAS.Core.Parameters.Rating;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.Rating
{
    public interface IRatingRepository
    {
        Task<Result<bool>> CreateRatingFeedbackAsync(ClaimsPrincipal principal, string appointmentId, CreateRatingRequest request);
        Task<PagedResult<RatingResponse>> GetAllRatingsAsync(string mentorId, RatingParameters param);
        Task<Result<RatingResponse>> GetRatingByIdAsync(string ratingId);
        Task<PagedResult<RatingResponse>> GetAllRatingsForAdminAsync(RatingParametersAdmin param);
        Task<Result<bool>> ProcessRatingAsync(string ratingId, ProcessRatingRequest request);
    }
}
