using System.Net;
using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.Slot;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Question;
using MAS.Core.Dtos.Outcoming.Slot;
using MAS.Core.Interfaces.Services.Question;
using MAS.Core.Interfaces.Services.Slot;
using MAS.Core.Parameters.Question;
using MAS.Core.Parameters.Slot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1;

[ApiVersion("1.0")]
public class SlotsController : BaseController
{
    private readonly ISlotService _slotService;
    private readonly IQuestionService _questionService;

    public SlotsController(ISlotService slotService, IQuestionService questionService)
    {
        _slotService = slotService;
        _questionService = questionService;
    }

    /// <summary>
    /// Get all available slots
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin, User
    /// </remarks>
    [HttpGet]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
    public async Task<ActionResult<PagedResult<SlotResponse>>> GetAllAvailableSlots([FromQuery] SlotParameters param)
    {
        var response = await _slotService.GetAllAvailableSlotsAsync(HttpContext.User, param);        
        return Ok(response);
    }

    /// <summary>
    /// Get slot by Id
    /// </summary>
    /// <param name="slotId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin, User
    /// </remarks>
    [HttpGet, Route("{slotId}")]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
    public async Task<ActionResult<Result<SlotDetailResponse>>> GetSlotById(string slotId)
    {
        var response = await _slotService.GetSlotByIdAsync(slotId);
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
    /// Create available slot
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpPost]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> CreateAvailableSlot(SlotCreateRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }
        var response = await _slotService.CreateAvailableSlotAsync(HttpContext.User, request);
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
    /// Delete slot
    /// </summary>
    /// <param name="slotId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpDelete, Route("{slotId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> DeleteSlot(string slotId)
    {
        var response = await _slotService.DeleteAvailableSlotAsync(HttpContext.User, slotId);
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
    /// Check which slot is passed
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin, User
    /// </remarks>
    [HttpPut]
    [Authorize(Roles = RoleConstants.User + "," + RoleConstants.Admin)]
    public async Task<ActionResult> CheckPassedSlot()
    {
        var response = await _slotService.CheckPassedSlotAsync();
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
    /// Get all Questions of specific slot
    /// </summary>
    /// <param name="slotId"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin, User
    /// </remarks>
    [HttpGet, Route("{slotId}/questions")]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
    public async Task<ActionResult<PagedResult<QuestionResponse>>> GetAllQuestions(
        string slotId,
        [FromQuery] QuestionParameters param)
    {
        var response = await _questionService.GetAllQuestionOfSlotAsync(slotId, param);
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
