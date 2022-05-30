using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.Major;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Major;
using MAS.Core.Interfaces.Services.Major;
using MAS.Core.Parameters.Major;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1
{
    [ApiVersion("1.0")]
    public class MajorsController : BaseController
    {
        private readonly IMajorService _majorService;

        public MajorsController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        /// <summary>
        /// Get all Majors
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
        public async Task<ActionResult<PagedResult<MajorResponse>>> GetAllMajors(
            [FromQuery] MajorParameters param)
        {
            var response = await _majorService.GetAllMajorsAsync(param);
            return Ok(response);
        }

        /// <summary>
        /// Get Major by Id
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{majorId}", Name = "GetMajorById")]
        [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
        public async Task<ActionResult<Result<MajorResponse>>> GetMajorById(string majorId)
        {
            var response = await _majorService.GetMajorByIdAsync(majorId);
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
        /// Create New a Major
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<Result<MajorResponse>>> CreateMajor(MajorCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _majorService.CreateMajorAsync(request);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }
            return CreatedAtRoute(nameof(GetMajorById), new { majorId = response.Content.Id }, response);
        }

        /// <summary>
        /// Update Major
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("{majorId}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult> UpdateMajor(string majorId, MajorUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _majorService.UpdateMajorAsync(majorId, request);
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
        /// Delete a Major
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpDelete("{majorId}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult> DeleteMajor(string majorId)
        {
            var response = await _majorService.DeleteMajorAsync(majorId);
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