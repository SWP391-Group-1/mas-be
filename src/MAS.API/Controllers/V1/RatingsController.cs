using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.Rating;
using MAS.Core.Interfaces.Services;
using MAS.Core.Parameters.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1;
[ApiVersion("1.0")]
public class RatingsController : BaseController
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    /// <summary>
    /// Make Rating Feedback 
    /// </summary>
    /// <param name="appointmentId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpPost("{appointmentId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> CreateRatingFeedback(string appointmentId, CreateRatingRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }
        var response = await _ratingService.CreateRatingFeedbackAsync(HttpContext.User, appointmentId, request);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }
        return Ok(response);
    }



    /// <summary>
    /// Get rating by Id
    /// </summary>
    /// <param name="ratingId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin, User
    /// </remarks>
    [HttpGet("{ratingId}")]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
    public async Task<ActionResult> GetRatingById(string ratingId)
    {
        var response = await _ratingService.GetRatingByIdAsync(ratingId);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }
        return Ok(response);
    }

    /// <summary>
    /// Get all unapproved ratings
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin
    /// </remarks>
    [HttpGet]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<ActionResult> GetAllRatings([FromQuery] RatingParametersAdmin param)
    {
        var response = await _ratingService.GetAllRatingsForAdminAsync(param);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }
        return Ok(response);
    }

    /// <summary>
    /// Process rating
    /// </summary>
    /// <param name="rateId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin
    /// </remarks>
    [HttpPut("process/{rateId}")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<ActionResult> ProcessViolateRating(string rateId, ProcessRatingRequest request)
    {
        var response = await _ratingService.ProcessViolateRatingAsync(rateId, request);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }
        return NoContent();
    }
}
