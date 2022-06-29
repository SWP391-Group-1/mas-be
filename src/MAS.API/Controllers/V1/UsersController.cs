using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.MasUser;
using MAS.Core.Dtos.Outcoming.Appointment;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.MasUser;
using MAS.Core.Interfaces.Services;
using MAS.Core.Interfaces.Services.Appointment;
using MAS.Core.Interfaces.Services.MasUser;
using MAS.Core.Parameters.Appointment;
using MAS.Core.Parameters.MasUser;
using MAS.Core.Parameters.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1;

[ApiVersion("1.0")]
public class UsersController : BaseController
{
    private readonly IMasUserService _masUserService;
    private readonly IAppointmentService _appointmentService;
    private readonly IRatingService _ratingService;

    public UsersController(
        IMasUserService masUserService,
        IAppointmentService appointmentService,
        IRatingService ratingService)
    {
        _masUserService = masUserService;
        _appointmentService = appointmentService;
        _ratingService = ratingService;
    }

    /*
    *================================================
    *                                              ||
    * UPDATE USER INFO APIs SECTION                ||
    *                                              ||
    *================================================
    */


    /// <summary>
    /// Update personal info in profile
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    /// 
    [HttpPut, Route("personal")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> UpdatePersonalInfo(UserPersonalInfoUpdateRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }
        var response = await _masUserService.UpdatePersonalInfoAsync(HttpContext.User, request);
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

    /*
    *================================================
    *                                              ||
    * GET USER INFO APIs SECTION                   ||
    *                                              ||
    *================================================
    */
    /// <summary>
    /// Get own personal information
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpGet, Route("personal")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult<Result<PersonalInfoResponse>>> GetPersonalInfo()
    {
        var response = await _masUserService.GetPersonalInfoByIdentityIdAsync(HttpContext.User);
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
    /// Get a specific user info
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User, Admin
    /// </remarks>
    [HttpGet, Route("{userId}")]
    [Authorize(Roles = RoleConstants.User + "," + RoleConstants.Admin)]
    public async Task<ActionResult<Result<UserGetBasicInfoResponse>>> GetUserBasicInfoById(string userId)
    {
        var response = await _masUserService.GetUserBasicInfoByIdAsync(userId);
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
    /// Get all mentors / Search mentors
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpGet, Route("mentors")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult<PagedResult<UserSearchResponse>>> GetAllMentor([FromQuery] UserParameters param)
    {
        var response = await _masUserService.GetAllMentorsAsync(HttpContext.User, param);

        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }

        var metaData = new {
            response.TotalCount,
            response.PageSize,
            response.CurrentPage,
            response.HasNext,
            response.HasPrevious
        };

        Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));

        return Ok(response);
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin
    /// </remarks>
    [HttpGet]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<ActionResult<PagedResult<UserGetByAdminResponse>>> GetAllUser([FromQuery] AdminUserParameters param)
    {
        var response = await _masUserService.GetAllUsersForAdminAsync(param);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }

        var metaData = new {
            response.TotalCount,
            response.PageSize,
            response.CurrentPage,
            response.HasNext,
            response.HasPrevious
        };

        Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));

        return Ok(response);
    }

    /// <summary>
    /// Active or disable user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin
    /// </remarks>
    [HttpPut, Route("active/{userId}")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<ActionResult> ActiveUser(string userId, IsActiveChangeRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }
        var response = await _masUserService.ChangeIsActiveUserForAdminAsync(userId, request);
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

    /// <summary>
    /// Send mentor request
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpPut, Route("mentor-request")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> MentorRequest()
    {
        var response = await _masUserService.SendMentorRequest(HttpContext.User);
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


    /// <summary>
    /// Accept mentor request
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin
    /// </remarks>
    [HttpPut, Route("accept-request")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<ActionResult> AcceptRequest(string userId, MentorRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }
        var response = await _masUserService.AcceptRequest(userId, request);
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

    /*
    *================================================
    *                                              ||
    * Appointment APIs Section                     ||
    *                                              ||
    *================================================
    */
    /// <summary>
    /// Get all own appointments
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpGet, Route("own/appointments")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult<PagedResult<AppointmentUserResponse>>> GetAllAppointmentsOfOwn([FromQuery] AppointmentUserParameters param)
    {
        var response = await _appointmentService.GetAllAppointmentsOfOwnAsync(HttpContext.User, param);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }
        var metaData = new {
            response.TotalCount,
            response.PageSize,
            response.CurrentPage,
            response.HasNext,
            response.HasPrevious
        };
        Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));
        return Ok(response);
    }

    /// <summary>
    /// Get all appointments for mentor
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpGet, Route("mentor/appointments")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult<PagedResult<AppointmentMentorResponse>>> GetAllAppointmentsOfMentor([FromQuery] AppointmentMentorParameters param)
    {
        var response = await _appointmentService.GetAllAppointmentsOfMentorAsync(HttpContext.User, param);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }
        var metaData = new {
            response.TotalCount,
            response.PageSize,
            response.CurrentPage,
            response.HasNext,
            response.HasPrevious
        };
        Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));
        return Ok(response);
    }

    /// <summary>
    /// Get all appointments of a user for admin
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin
    /// </remarks>
    [HttpGet, Route("{userId}/appointments")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<ActionResult<PagedResult<AppointmentAdminResponse>>> GetAllAppointmentsOfMentorForAdmin(
        string userId,
        [FromQuery] AppointmentAdminParameters param)
    {
        var response = await _appointmentService.GetAllAppointmentsOfUserForAdminAsync(userId, param);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }
        var metaData = new {
            response.TotalCount,
            response.PageSize,
            response.CurrentPage,
            response.HasNext,
            response.HasPrevious
        };
        Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));
        return Ok(response);
    }

    /// <summary>
    /// Get a specific appointment create by self
    /// </summary>
    /// <param name="appointmentId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User (normal User)
    /// </remarks>
    [HttpGet, Route("own/appointments/{appointmentId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult<Result<AppointmentUserDetailResponse>>> GetAppointmentOfOwnById(string appointmentId)
    {
        var response = await _appointmentService.GetAppointmentOfOwnByIdAsync(HttpContext.User, appointmentId);
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
    /// Get a specific appointment received
    /// </summary>
    /// <param name="appointmentId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User (Mentor)
    /// </remarks>
    [HttpGet, Route("mentor/appointments/{appointmentId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult<Result<AppointmentMentorDetailResponse>>> GetAppointmentOfMentorById(string appointmentId)
    {
        var response = await _appointmentService.GetAppointmentOfMentorByIdAsync(HttpContext.User, appointmentId);
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
    /// Get all rating of a specific mentor
    /// </summary>
    /// <param name="mentorId"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin, User
    /// </remarks>
    [HttpGet, Route("{mentorId}/ratings")]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
    public async Task<ActionResult> GetAllRatings(string mentorId, [FromQuery] RatingParameters param)
    {
        var response = await _ratingService.GetAllRatingsAsync(mentorId, param);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }
        var metaData = new {
            response.TotalCount,
            response.PageSize,
            response.CurrentPage,
            response.HasNext,
            response.HasPrevious
        };

        Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));

        return Ok(response);
    }


}
