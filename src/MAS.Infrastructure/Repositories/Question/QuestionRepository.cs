﻿using AutoMapper;
using MAS.Core.Constants.Error;
using MAS.Core.Dtos.Incoming.Question;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Question;
using MAS.Core.Interfaces.Repositories.Question;
using MAS.Core.Parameters.Question;
using MAS.Infrastructure.Data;
using MAS.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.Question;

public class QuestionRepository : BaseRepository, IQuestionRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    public QuestionRepository(IMapper mapper,
                              AppDbContext context,
                              UserManager<IdentityUser> userManager) : base(mapper, context)
    {
        _userManager = userManager;
    }
    public async Task<Result<QuestionResponse>> AnswerQuestionAsync(
        ClaimsPrincipal principal,
        string questionId,
        AnswerQuestionRequest request)
    {
        var result = new Result<QuestionResponse>();
        var loggedInUser = await _userManager.GetUserAsync(principal);
        if (loggedInUser is null) {
            result.Error = ErrorHelper.PopulateError(400,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotAllowModify);
            return result;
        }

        var question = await _context.Questions.FindAsync(questionId);
        if (question == null) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "question.");
            return result;
        }

        await _context.Entry(question).Reference(x => x.Appointment).LoadAsync();
        var mentor = await _context.MasUsers.FindAsync(question.Appointment.MentorId);
        if (mentor is null) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotLogIn);
            return result;
        }
        if (mentor.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.AccountDisable);
            return result;
        }
        if (loggedInUser.Id != mentor.IdentityId) {
            result.Error = ErrorHelper.PopulateError(400,
                                                    ErrorTypes.BadRequest,
                                                    "Do not have permission to perform this.");
            return result;
        }

        var model = _mapper.Map(request, question);
        _context.Questions.Update(model);

        if (await _context.SaveChangesAsync() >= 0) {
            var response = _mapper.Map<QuestionResponse>(model);
            result.Content = response;
            return result;
        }
        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                 ErrorTypes.SaveFail,
                                                 ErrorMessages.SaveFail);
        return result;
    }

    public async Task<Result<QuestionResponse>> CreateQuestionAsync(
        ClaimsPrincipal principal,
        CreateQuestionRequest request)
    {
        var result = new Result<QuestionResponse>();

        var loggedInUser = await _userManager.GetUserAsync(principal);
        if (loggedInUser is null) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotLogIn);
            return result;
        }
        var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

        var user = await _context.MasUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
        if (user is null) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotLogIn);
            return result;
        }

        if (user.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.AccountDisable);
            return result;
        }

        var appointment = await _context.Appointments.FindAsync(request.AppointmentId);
        if (appointment == null) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + $"this Appointment in System.");
            return result;
        }

        if(appointment.CreatorId != user.Id){
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     $"You are not creator of this appointment so you not allowed to add question!");
            return result;
        }

        var model = _mapper.Map<Core.Entities.Question>(request);
        model.CreatorId = user.Id;
        await _context.Questions.AddAsync(model);

        if ((await _context.SaveChangesAsync() >= 0)) {
            var response = _mapper.Map<QuestionResponse>(model);
            result.Content = response;
            return result;
        }
        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                 ErrorTypes.SaveFail,
                                                 ErrorMessages.SaveFail);
        return result;
    }

    public async Task<Result<bool>> DeleteQuestionAsync(
        ClaimsPrincipal principal,
        string questionId)
    {
        var result = new Result<bool>();

        var loggedInUser = await _userManager.GetUserAsync(principal);
        if (loggedInUser is null) {
            result.Error = ErrorHelper.PopulateError(400,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotAllowModify);
            return result;
        }

        var question = await _context.Questions.FindAsync(questionId);
        if (question == null || question.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "question.");
            return result;
        }

        var user = await _context.MasUsers.FindAsync(question.CreatorId);
        if (user is null) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotLogIn);
            return result;
        }
        if (user.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.AccountDisable);
            return result;
        }

        if (loggedInUser.Id != user.IdentityId) {
            result.Error = ErrorHelper.PopulateError(400,
                                                     ErrorTypes.BadRequest,
                                                     "Do not have permission to perform this.");
            return result;
        }
        question.IsActive = false;
        if ((await _context.SaveChangesAsync() >= 0)) {
            result.Content = true;
            return result;
        }
        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                 ErrorTypes.SaveFail,
                                                 ErrorMessages.SaveFail);
        return result;
    }

    public async Task<PagedResult<QuestionResponse>> GetAllQuestionAsync(
        string appointmentId,
        QuestionParameters param)
    {
        var result = new PagedResult<QuestionResponse>();

        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null || appointment.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotFound + "appointment!");
            return result;
        }

        var questions = await _context.Questions.Where(x => x.AppointmentId == appointmentId).ToListAsync();
        var query = questions.AsQueryable();
        FilterActive(ref query, param.IsActive);
        FilterNewQuestion(ref query, param.IsNew);
        questions = query.ToList();
        var response = _mapper.Map<List<QuestionResponse>>(questions);
        return PagedResult<QuestionResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
    }

    private void FilterNewQuestion(ref IQueryable<Core.Entities.Question> query, bool? isNew)
    {
        if (!query.Any() || isNew is null) {
            return;
        }
        if (isNew is true) {
            query = query.OrderByDescending(x => x.CreateDate);
        }
        else {
            query = query.OrderBy(x => x.CreateDate);
        }
    }

    private void FilterActive(ref IQueryable<Core.Entities.Question> query, bool? isActive)
    {
        if (!query.Any() || isActive is null) {
            return;
        }
        query = query.Where(x => x.IsActive == isActive);
    }

    public async Task<Result<QuestionResponse>> GetQuestionByIdAsync(string questionId)
    {
        var result = new Result<QuestionResponse>();

        var question = await _context.Questions.FindAsync(questionId);
        if (question == null || question.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "question.");
            return result;
        }
        var response = _mapper.Map<QuestionResponse>(question);
        result.Content = response;
        return result;
    }

    public async Task<PagedResult<QuestionResponse>> GetAllQuestionOfSlotAsync(string slotId, QuestionParameters param)
    {
        var result = new PagedResult<QuestionResponse>();

        var slot = await _context.Slots.FindAsync(slotId);
        if (slot == null || slot.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotFound + "slot!");
            return result;
        }

        var questions = await _context.Questions.Where(x => x.Appointment.SlotId == slotId).ToListAsync();
        var query = questions.AsQueryable();
        FilterActive(ref query, param.IsActive);
        FilterNewQuestion(ref query, param.IsNew);
        questions = query.ToList();
        var response = _mapper.Map<List<QuestionResponse>>(questions);
        return PagedResult<QuestionResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
    }
}
