using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.Subject;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Subject;
using MAS.Core.Interfaces.Services.Subject;
using MAS.Core.Parameters.Subject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1
{
    [ApiVersion("1.0")]
    public class SubjectsController : BaseController
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>
        /// Get all Subjects
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
        public async Task<ActionResult<PagedResult<SubjectResponse>>> GetAllSubjects(
            [FromQuery] SubjectParameters param)
        {
            var response = await _subjectService.GetAllSubjectsAsync(param);
            return Ok(response);
        }

        /// <summary>
        /// Get Subject by Id
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{subjectId}", Name = "GetSubjectById")]
        [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
        public async Task<ActionResult<Result<SubjectDetailResponse>>> GetSubjectById(string subjectId)
        {
            var response = await _subjectService.GetSubjectByIdAsync(subjectId);
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
        /// Create New a Subject
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<Result<SubjectResponse>>> CreateSubject(SubjectCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _subjectService.CreateSubjectAsync(request);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }
            return CreatedAtRoute(nameof(GetSubjectById), new { subjectId = response.Content.Id }, response);
        }

        /// <summary>
        /// Update Subject
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("{subjectId}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult> UpdateSubject(string subjectId, SubjectUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _subjectService.UpdateSubjectAsync(subjectId, request);
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
        /// Delete a Subject
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpDelete("{subjectId}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult> DeleteSubject(string subjectId)
        {
            var response = await _subjectService.DeleteSubjectAsync(subjectId);
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
}
