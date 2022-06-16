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

    public async Task<Result<bool>> CreateAvailableSlotAsync(ClaimsPrincipal principal, SlotCreateRequest request)
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

        if (request.StartTime <= request.FinishTime) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "Finish Time invalid!");
            return result;
        }

        if (request.CreateDate > request.StartTime.AddDays(-2)) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This slot must be started after today at least 2 days!");
            return result;
        }

        var mentorSlots = await _context.Slots.Where(x => x.MentorId == user.Id && x.IsActive == true).ToListAsync();
        foreach (var slot in mentorSlots) {
            if (slot.StartTime <= request.StartTime && slot.FinishTime >= request.StartTime) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     $"Existing slot from {slot.StartTime} to {slot.FinishTime}.");
                return result;
            }

            if (slot.StartTime <= request.FinishTime && slot.FinishTime >= request.FinishTime) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     $"Existing slot from {slot.StartTime} to {slot.FinishTime}.");
                return result;
            }

            if (request.StartTime <= slot.StartTime && request.FinishTime >= slot.FinishTime) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     $"Existing slot from {slot.StartTime} to {slot.FinishTime}.");
                return result;
            }
        }

        var model = _mapper.Map<Core.Entities.Slot>(request);
        model.MentorId = user.Id;
        await _context.Slots.AddAsync(model);

        if ((await _context.SaveChangesAsync() >= 0)) {
            result.Content = true;
            return result;
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

    public async Task<PagedResult<SlotResponse>> GetAllAvailableSlotsAsync(SlotParameters param)
    {
        var result = new PagedResult<SlotResponse>();

        var slots = await _context.Slots.ToListAsync();
        var query = slots.AsQueryable();
        FilterSlotByMentorId(ref query, param.MentorId);
        FilterByRange(ref query, param.From, param.To);
        SortByAsc(ref query, param.IsAsc);
        FilterActive(ref query, param.IsActive);

        slots = query.ToList();
        var response = _mapper.Map<List<SlotResponse>>(slots);
        return PagedResult<SlotResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
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
        if (!query.Any() || from is null || to is null) {
            return;
        }

        if (from >= to) {
            return;
        }

        query = query.Where(x => x.StartTime >= from && x.StartTime <= to);
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
        if (!query.Any() || !string.IsNullOrEmpty(mentorId) || !string.IsNullOrWhiteSpace(mentorId)) {
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
                                                     ErrorMessages.NotFound + "subject.");
            return result;
        }
        await _context.Entry(slot).Reference(x => x.Mentor).LoadAsync();
        var response = _mapper.Map<SlotDetailResponse>(slot);
        result.Content = response;
        return result;
    }
}
