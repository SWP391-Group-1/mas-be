using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.Appointment;
using MAS.Core.Dtos.Outcoming.Appointment;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Question;
using MAS.Core.Interfaces.Services.Appointment;
using MAS.Core.Interfaces.Services.Question;
using MAS.Core.Parameters.Question;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1;

[ApiVersion("1.0")]
public class AppointmentsController : BaseController
{
    private readonly IAppointmentService _appointmentService;
    private readonly IQuestionService _questionService;

    public AppointmentsController(
        IAppointmentService appointmentService,
        IQuestionService questionService)
    {
        _appointmentService = appointmentService;
        _questionService = questionService;
    }

    /// <summary>
    /// Get a specific appointment (Only for Admin)
    /// </summary>
    /// <param name="appointmentId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin
    /// </remarks>
    [HttpGet, Route("{appointmentId}")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<ActionResult<Result<AppointmentAdminDetailResponse>>> GetAppointmentByIdForAdmin(string appointmentId)
    {
        var response = await _appointmentService.GetAppointmentByIdForAdminAsync(appointmentId);
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
    /// Create appointment
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpPost]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> CreateAppointment(AppointmentCreateRequest request)
    {
        var response = await _appointmentService.CreateAppointmentAsync(HttpContext.User, request);
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
    /// Delete appointment
    /// </summary>
    /// <param name="appointmentId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpDelete, Route("{appointmentId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> DeleteAppointment(string appointmentId)
    {
        var response = await _appointmentService.DeleteAppointmentAsync(HttpContext.User, appointmentId);
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
    /// Process appointment 
    /// </summary>
    /// <param name="appointmentId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User (Mentor)
    /// </remarks>
    [HttpPut, Route("process/{appointmentId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> ProcessAppointment(string appointmentId, AppointmentProcessRequest request)
    {
        var response = await _appointmentService.ProcessAppointmentAsync(HttpContext.User, appointmentId, request);
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
    /// Update to describe appointment by mentor 
    /// </summary>
    /// <param name="appointmentId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User (Mentor)
    /// </remarks>
    [HttpPut, Route("update/{appointmentId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> MentorUpdateAppointment(string appointmentId, AppointmentUpdateRequest request)
    {
        var response = await _appointmentService.MentorUpdateAppointmentAsync(HttpContext.User, appointmentId, request);
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
    /// Get all Questions of specific appointment
    /// </summary>
    /// <param name="appointmentId"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin, User
    /// </remarks>
    [HttpGet, Route("{appointmentId}/questions")]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
    public async Task<ActionResult<PagedResult<QuestionResponse>>> GetAllQuestions(
        string appointmentId,
        [FromQuery] QuestionParameters param)
    {
        var response = await _questionService.GetAllQuestionAsync(appointmentId, param);
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
}
