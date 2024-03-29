﻿using MAS.API.Helpers;
using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.MentorSubject;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.MentorSubject;
using MAS.Core.Interfaces.Services.MentorSubject;
using MAS.Core.Parameters.MentorSubject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/mentor-subjects")]
public class MentorSubjectsController : BaseController
{
    private readonly IMentorSubjectService _mentorSubjectService;

    public MentorSubjectsController(IMentorSubjectService mentorSubjectService)
    {
        _mentorSubjectService = mentorSubjectService;
    }


    /// <summary>
    /// Get all Subjects of a specific Mentor
    /// </summary>
    /// <param name="mentorId"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin, User
    /// </remarks>
    [HttpGet, Route("{mentorId}")]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
    public async Task<ActionResult<PagedResult<MentorSubjectResponse>>> GetAllSubjectsOfMentor(
        string mentorId,
        [FromQuery] MentorSubjectParameters param)
    {
        var response = await _mentorSubjectService.GetAllSubjectsOfMentorAsync(mentorId, param);
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
    /// Register a Subject into Mentor Profile
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: user
    /// </remarks>
    [HttpPost]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult<Result<bool>>> RegisterSubject(MentorSubjectRegisterRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }
        var response = await _mentorSubjectService.RegisterSubjectAsync(HttpContext.User, request);
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
    /// Update Subject info of Mentor
    /// </summary>
    /// <param name="subjectOfMentorId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpPut, Route("{subjectOfMentorId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> UpdateSubjectOfMentor(string subjectOfMentorId, MentorSubjectUpdateRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }
        var response = await _mentorSubjectService.UpdateSubjectOfMentorAsync(HttpContext.User, subjectOfMentorId, request);
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
    /// Delete a Subject of Mentor
    /// </summary>
    /// <param name="subjectOfMentorId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpDelete("{subjectOfMentorId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> DeleteSubjectOfMentor(string subjectOfMentorId)
    {
        var response = await _mentorSubjectService.DeleteSubjectOfMentorAsync(HttpContext.User, subjectOfMentorId);
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
