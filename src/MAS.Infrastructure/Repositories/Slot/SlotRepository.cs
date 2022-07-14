using AutoMapper;
using MAS.Core.Constants.Error;
using MAS.Core.Dtos.Incoming.Slot;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Slot;
using MAS.Core.Interfaces.Repositories.Slot;
using MAS.Core.Parameters.Slot;
using MAS.Infrastructure.Data;
using MAS.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.Slot;

public class SlotRepository : BaseRepository, ISlotRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    public SlotRepository(IMapper mapper, AppDbContext context, UserManager<IdentityUser> userManager) : base(mapper, context)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> CreateAvailableSlotAsync(
        ClaimsPrincipal principal,
        SlotCreateRequest request)
    {
        var result = new Result<bool>();
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

        if (user.IsMentor is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "You are not Mentor!");
            return result;
        }

        if (String.IsNullOrEmpty(user.MeetUrl) || String.IsNullOrWhiteSpace(user.MeetUrl)) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "Please input your Google Meet URL!");
            return result;
        }

        await _context.Entry(user).Collection(x => x.MentorSubjects).LoadAsync();
        if (user.MentorSubjects.Count() == 0) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "Please register your major Subject!");
            return result;
        }

        if (request.StartTime < request.CreateDate.AddHours(-7)) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "Start time must be greater than now!");
            return result;
        }

        if (request.CreateDate.AddHours(-7) > request.StartTime.AddHours(-2)) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This slot must be started after now at least 2 hours!");
            return result;
        }

        if (request.CreateDate.AddHours(-7).AddYears(1) < request.StartTime) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "A slot must be created within 1 year!");
            return result;
        }

        if (request.StartTime >= request.FinishTime) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "Finish Time invalid!");
            return result;
        }

        if (request.FinishTime.AddMinutes(-30) <= request.StartTime) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "A Slot have at least 30 minutes!");
            return result;
        }

        if (request.StartTime.AddHours(24) < request.FinishTime) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "A Slot not more than 24 hours!");
            return result;
        }

        var mentorSlots = await _context.Slots.Where(x => x.MentorId == user.Id && x.IsActive == true).ToListAsync();
        foreach (var slot in mentorSlots) {
            if (slot.StartTime <= request.StartTime && slot.FinishTime >= request.StartTime) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     $"Existing slot from {slot.StartTime.AddHours(7)} to {slot.FinishTime.AddHours(7)}.");
                return result;
            }

            if (slot.StartTime <= request.FinishTime && slot.FinishTime >= request.FinishTime) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     $"Existing slot from {slot.StartTime.AddHours(7)} to {slot.FinishTime.AddHours(7)}.");
                return result;
            }

            if (request.StartTime <= slot.StartTime && request.FinishTime >= slot.FinishTime) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     $"Existing slot from {slot.StartTime.AddHours(7)} to {slot.FinishTime.AddHours(7)}.");
                return result;
            }
        }

        var model = _mapper.Map<Core.Entities.Slot>(request);
        model.MentorId = user.Id;
        model.IsPassed = false;

        foreach (var item in request.SlotSubjects) {
            if (!await _context.Subjects.AnyAsync(x => x.Id == item.SubjectId && x.IsActive == true)) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     $"Not found subject with id {item.Id}");
                return result;
            }
            if (!await _context.MentorSubjects.AnyAsync(x => x.MentorId == user.Id
                                                             && x.SubjectId == item.SubjectId
                                                             && x.IsActive == true)) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     $"You don't register this subject in your profile!");
                return result;
            }
        }

        foreach (var item in model.SlotSubjects) {
            item.SlotId = model.Id;
        }

        // var slotSubjects = _mapper.Map<ICollection<Core.Entities.SlotSubject>>(request.SlotSubjects);
        // foreach (var item in slotSubjects)
        // {
        //     item.SlotId = model.Id;
        // }

        await _context.AddAsync(model);

        if ((await _context.SaveChangesAsync() >= 0)) {
            // await _context.SlotSubjects.AddRangeAsync(slotSubjects);
            // if ((await _context.SaveChangesAsync() >= 0)) {
            result.Content = true;
            return result;
            // }
        }
        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                 ErrorTypes.SaveFail,
                                                 ErrorMessages.SaveFail);
        return result;
    }

    public async Task<Result<bool>> DeleteAvailableSlotAsync(ClaimsPrincipal principal, string slotId)
    {
        var result = new Result<bool>();
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

        var slot = await _context.Slots.FindAsync(slotId);
        if (slot is null || slot.IsActive == false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotFound + "this information!");
            return result;
        }

        if (slot.IsPassed is true) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This Slot is passed!");
            return result;
        }

        if (slot.MentorId != user.Id) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotAllowModify);
            return result;
        }

        await _context.Entry(slot).Collection(x => x.Appointments).LoadAsync();
        if (slot.Appointments.Count() > 0) {
            foreach (var item in slot.Appointments) {
                if (item.IsApprove is true) {
                    result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "Have appointments in this slot!");
                    return result;
                }
            }
        }

        slot.IsActive = false;
        if ((await _context.SaveChangesAsync() >= 0)) {
            foreach (var item in slot.Appointments) {
                if (item.IsApprove is not true) {
                    item.IsActive = false;
                    if ((await _context.SaveChangesAsync() < 0)) {
                        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                             ErrorTypes.SaveFail,
                                             ErrorMessages.SaveFail);
                        return result;
                    }
                }
            }
            result.Content = true;
            return result;
        }
        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                 ErrorTypes.SaveFail,
                                                 ErrorMessages.SaveFail);
        return result;
    }

    public async Task<PagedResult<SlotResponse>> GetAllAvailableSlotsAsync(
        ClaimsPrincipal principal,
        SlotParameters param)
    {
        var result = new PagedResult<SlotResponse>();

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

        var slots = await _context.Slots.ToListAsync();
        var query = slots.AsQueryable();
        FilterSlotByMentorId(ref query, param.MentorId);
        FilterByRange(ref query, param.From, param.To);
        FilterPassedSlots(ref query, param.IsPassed);
        FilterActive(ref query, param.IsActive);
        SortByAsc(ref query, param.IsAsc);

        slots = query.ToList();
        foreach (var item in slots) {
            await _context.Entry(item).Reference(x => x.Mentor).LoadAsync();
            await _context.Entry(item).Collection(x => x.SlotSubjects).Query().Include(x => x.Subject).LoadAsync();
        }
        var response = _mapper.Map<List<SlotResponse>>(slots);
        foreach (var item in response) {
            item.NumOfAppointments = await _context.Appointments
                                                        .Where(x => x.SlotId == item.Id
                                                                    && x.IsApprove == true)
                                                        .CountAsync();
            item.IsIn = await _context.Appointments
                                            .Where(x => x.SlotId == item.Id
                                                        && x.IsActive == true
                                                        && x.CreatorId == user.Id)
                                            .AnyAsync();
            item.IsOwn = (item.MentorId == user.Id);

        }
        var resQuery = response.AsQueryable();
        FilterIsIn(ref resQuery, param.IsIn);
        FilterIsOwn(ref resQuery, param.IsOwn);
        response = resQuery.ToList();
        return PagedResult<SlotResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
    }

    private void FilterIsOwn(ref IQueryable<SlotResponse> resQuery, bool? isOwn)
    {
        if (!resQuery.Any() || isOwn is null) {
            return;
        }
        resQuery = resQuery.Where(x => x.IsOwn == isOwn);
    }

    private void FilterIsIn(ref IQueryable<SlotResponse> resQuery, bool? isIn)
    {
        if (!resQuery.Any() || isIn is null) {
            return;
        }
        resQuery = resQuery.Where(x => x.IsIn == isIn);
    }

    private void FilterPassedSlots(ref IQueryable<Core.Entities.Slot> query, bool? isPassed)
    {
        if (!query.Any() || isPassed is null) {
            return;
        }
        query = query.Where(x => x.IsPassed == isPassed);
    }

    private void FilterActive(ref IQueryable<Core.Entities.Slot> query, bool? isActive)
    {
        if (!query.Any() || isActive is null) {
            return;
        }
        query = query.Where(x => x.IsActive == isActive);
    }

    private void FilterByRange(ref IQueryable<Core.Entities.Slot> query, DateTime? from, DateTime? to)
    {
        if (!query.Any() || (from is null && to is null)) {
            return;
        }

        if (from >= to) {
            return;
        }

        if (from is not null && to is not null) {
            query = query.Where(x => x.StartTime >= from && x.StartTime <= to);
        }
        else if (from is null && to is not null) {
            query = query.Where(x => x.StartTime <= to);
        }
        else if (from is not null && to is null) {
            query = query.Where(x => x.StartTime >= from);
        }
    }

    private void SortByAsc(ref IQueryable<Core.Entities.Slot> query, bool? isAsc)
    {
        if (!query.Any() || isAsc is null) {
            return;
        }
        if (isAsc is true) {
            query = query.OrderBy(x => x.StartTime);
        }
        else {
            query = query.OrderByDescending(x => x.StartTime);
        }
    }

    private void FilterSlotByMentorId(ref IQueryable<Core.Entities.Slot> query, string mentorId)
    {
        if (!query.Any() || string.IsNullOrEmpty(mentorId) || string.IsNullOrWhiteSpace(mentorId)) {
            return;
        }
        query = query.Where(x => x.MentorId == mentorId);
    }

    public async Task<Result<SlotDetailResponse>> GetSlotByIdAsync(string slotId)
    {
        var result = new Result<SlotDetailResponse>();

        var slot = await _context.Slots.FindAsync(slotId);
        if (slot == null || slot.IsActive == false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "slot.");
            return result;
        }
        await _context.Entry(slot).Reference(x => x.Mentor).LoadAsync();
        await _context.Entry(slot).Collection(x => x.SlotSubjects).Query().Include(x => x.Subject).LoadAsync();
        var response = _mapper.Map<SlotDetailResponse>(slot);
        response.NumOfAppointments = await _context.Appointments
                                                        .Where(x => x.SlotId == slot.Id
                                                                    && x.IsApprove == true)
                                                        .CountAsync();
        result.Content = response;
        return result;
    }

    public async Task<Result<bool>> CheckPassedSlotAsync()
    {
        var result = new Result<bool>();
        var now = DateTime.UtcNow.AddHours(7);

        var slots = await _context.Slots.ToListAsync();
        foreach (var item in slots) {
            if (now > item.FinishTime.AddHours(7)) {
                item.IsPassed = true;
            }
        }

        if ((await _context.SaveChangesAsync() >= 0)) {
            result.Content = true;
            return result;
        }
        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                 ErrorTypes.SaveFail,
                                                 ErrorMessages.SaveFail);
        return result;
    }
}
