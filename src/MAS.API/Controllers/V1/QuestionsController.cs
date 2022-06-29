using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Incoming.Question;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Question;
using MAS.Core.Interfaces.Services.Question;
using MAS.Core.Parameters.Question;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MAS.API.Controllers.V1;

[ApiVersion("1.0")]
public class QuestionsController : BaseController
{

    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }    

    /// <summary>
    /// Get Question by Id
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin, User
    /// </remarks>
    [HttpGet("{questionId}", Name = "GetQuestionById")]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.User)]
    public async Task<ActionResult<Result<QuestionResponse>>> GetQuestionById(string questionId)
    {
        var response = await _questionService.GetQuestionById(questionId);
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
    /// Create New a Question
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpPost]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult<Result<QuestionResponse>>> CreateQuestion(CreateQuestionRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }
        var response = await _questionService.CreateQuestionAsync(HttpContext.User, request);
        if (!response.IsSuccess) {
            if (response.Error.Code == 404) {
                return NotFound(response);
            }
            else {
                return BadRequest(response);
            }
        }
        return CreatedAtRoute(nameof(GetQuestionById), new { questionId = response.Content.Id }, response);
    }

    /// <summary>
    /// Answer Question
    /// </summary>
    /// <param name="questionId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: User
    /// </remarks>
    [HttpPut, Route("{questionId}")]
    [Authorize(Roles = RoleConstants.User)]
    public async Task<ActionResult> AnswerQuestion(string questionId, AnswerQuestionRequest request)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }
        var response = await _questionService.AnswerQuestion(HttpContext.User, questionId, request);
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
    /// Delete a Question
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    /// <remarks>
    /// Roles Access: Admin
    /// </remarks>
    [HttpDelete("{questionId}")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<ActionResult> DeleteQuestion(string questionId)
    {
        var response = await _questionService.DeleteQuestion(HttpContext.User, questionId);
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
