using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.Subject;
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
        private readonly ISubjectService subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
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
        public async Task<ActionResult> GetAllSubjects(
            [FromQuery] SubjectParameters param)
        {
            var response = await subjectService.GetAllSubjectsAsync(param);
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
        /// Get Subject by Id
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{subjectId}", Name = "GetSubjectById")]
        [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
        public async Task<ActionResult> GetSubjectById(string subjectId)
        {
            var response = await subjectService.GetSubjectByIdAsync(subjectId);
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
        public async Task<ActionResult> CreateSubject(SubjectCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await subjectService.CreateSubjectAsync(request);
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
            var response = await subjectService.UpdateSubjectAsync(subjectId, request);
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
            var response = await subjectService.DeleteSubjectAsync(subjectId);
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
