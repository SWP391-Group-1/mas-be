using AutoMapper;
using MAS.Core.Constants.Appointment;
using MAS.Core.Constants.Error;
using MAS.Core.Dtos.Incoming.Appointment;
using MAS.Core.Dtos.Outcoming.Appointment;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Interfaces.Repositories.Appointment;
using MAS.Core.Parameters.Appointment;
using MAS.Infrastructure.Data;
using MAS.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.Appointment;

public class AppointmentRepository : BaseRepository, IAppointmentRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    public AppointmentRepository(IMapper mapper,
                                 AppDbContext context,
                                 UserManager<IdentityUser> userManager) : base(mapper, context)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> CreateAppointmentAsync(
        ClaimsPrincipal principal,
        AppointmentCreateRequest request)
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

        var slot = await _context.Slots.FindAsync(request.SlotId);
        if (slot is null || slot.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "Slot");
            return result;
        }

        if (slot.StartTime < DateTime.UtcNow.AddHours(7)) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "Slot is passed");
            return result;
        }

        if (slot.StartTime.AddHours(-(int)AppointmentTime.TimeProcess) < request.CreateDate) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     "Not allow to register appointment in this slot!. You can register a slot before it start at least 1 hour!");
            return result;
        }

        if (slot.MentorId == user.Id) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This slot is your slot!");
            return result;
        }

        var existAppointments = await _context.Appointments.Where(x => x.CreatorId == user.Id
                                                                       && x.IsApprove != false).ToListAsync();
        foreach (var item in existAppointments) {
            if (item.StartTime >= slot.StartTime && item.StartTime <= slot.FinishTime) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.Exist
                                                     + $"appointment at {slot.StartTime} to {slot.FinishTime}.");
                return result;
            }

            if (item.FinishTime >= slot.StartTime && item.FinishTime <= slot.FinishTime) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.Exist
                                                     + $"appointment at {slot.StartTime} to {slot.FinishTime}.");
                return result;
            }

            if (item.StartTime <= slot.StartTime && item.FinishTime >= slot.FinishTime) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.Exist
                                                     + $"appointment at {slot.StartTime} to {slot.FinishTime}.");
                return result;
            }
        }

        var deniedAppointments = await _context.Appointments.AnyAsync(x => x.CreatorId == user.Id
                                                                        && x.SlotId == slot.Id
                                                                       && x.IsApprove == false);

        if (deniedAppointments) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "You have appointment in this slot and your appointment is denied!");
            return result;
        }

        await _context.Appointments.AddAsync(
            new Core.Entities.Appointment {
                Id = request.Id,
                CreateDate = request.CreateDate,
                UpdateDate = null,
                CreatorId = user.Id,
                MentorId = slot.MentorId,
                SlotId = request.SlotId,
                BriefProblem = request.BriefProblem,
                IsApprove = null,
                StartTime = slot.StartTime,
                FinishTime = slot.FinishTime,
                MentorDescription = "",
                IsPassed = false,
                IsActive = true
            }
        );
        if ((await _context.SaveChangesAsync() >= 0)) {
            result.Content = true;
            return result;
        }
        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                 ErrorTypes.SaveFail,
                                                 ErrorMessages.SaveFail);
        return result;
    }

    public async Task<Result<bool>> DeleteAppointmentAsync(
        ClaimsPrincipal principal,
        string appointmentId)
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

        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment is null || appointment.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "Appointment");
            return result;
        }
        if (appointment.CreatorId != user.Id) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.NotAllowModify);
            return result;
        }
        if (appointment.IsApprove is not null) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This appointment is processed by Mentor");
            return result;
        }

        // await _context.Entry(appointment).Collection(x => x.AppointmentSubjects).LoadAsync();
        // _context.AppointmentSubjects.RemoveRange(appointment.AppointmentSubjects);
        // if ((await _context.SaveChangesAsync() >= 0)) {
        appointment.IsActive = false;
        if ((await _context.SaveChangesAsync() >= 0)) {
            result.Content = true;
            return result;
        }
        // }
        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                 ErrorTypes.SaveFail,
                                                 ErrorMessages.SaveFail);
        return result;
    }

    public async Task<PagedResult<AppointmentMentorResponse>> GetAllReceiveAppointmentsAsync(
        ClaimsPrincipal principal,
        AppointmentMentorParameters param)
    {
        var result = new PagedResult<AppointmentMentorResponse>();

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
        if (user.IsMentor is not true) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "You are not mentor!");
            return result;
        }

        var apps = await _context.Appointments.Where(x => x.MentorId == user.Id).ToListAsync();

        var query = apps.AsQueryable();

        FilterBySlot(ref query, param.SlotId);
        SortNewAppointment(ref query, param.IsNew);
        FilterActive(ref query, param.IsActive);
        FilterIsPassed(ref query, param.IsPassed);
        FilterApprove(ref query, param.IsApprove, param.IsAll);

        apps = query.ToList();
        foreach (var item in apps) {
            await _context.Entry(item).Reference(x => x.Slot).LoadAsync();
        }
        var response = _mapper.Map<List<AppointmentMentorResponse>>(apps);
        foreach (var item in response) {
            item.Creator = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(item.CreatorId));
            item.Slot.NumOfAppointments = await _context.Appointments
                                                        .Where(x => x.SlotId == item.SlotId
                                                                    && x.IsApprove == true)
                                                        .CountAsync();
        }
        return PagedResult<AppointmentMentorResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
    }

    private void FilterActive(ref IQueryable<Core.Entities.Appointment> query, bool? isActive)
    {
        if (!query.Any() || isActive is null) {
            return;
        }
        query = query.Where(x => x.IsActive == isActive);
    }
    private void FilterApprove(ref IQueryable<Core.Entities.Appointment> query, bool? isApprove, bool? isAll)
    {
        if (!query.Any() || isAll is true || isAll is null) {
            return;
        }
        query = query.Where(x => x.IsApprove == isApprove);
    }

    private void FilterBySlot(ref IQueryable<Core.Entities.Appointment> query, string slotId)
    {
        if (!query.Any() || String.IsNullOrEmpty(slotId) || String.IsNullOrWhiteSpace(slotId)) {
            return;
        }
        query = query.Where(x => x.SlotId == slotId);
    }

    private void SortNewAppointment(ref IQueryable<Core.Entities.Appointment> query, bool? isNew)
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

    public async Task<PagedResult<AppointmentAdminResponse>> GetAllAppointmentsOfUserForAdminAsync(
        string userId,
        AppointmentAdminParameters param)
    {
        var apps = await _context.Appointments.Where(x => (x.MentorId + x.CreatorId).Contains(userId)).ToListAsync();

        var query = apps.AsQueryable();

        FilterBySlot(ref query, param.SlotId);
        SortNewAppointment(ref query, param.IsNew);
        FilterActive(ref query, param.IsActive);
        FilterIsPassed(ref query, param.IsPassed);

        apps = query.ToList();
        foreach (var item in apps) {
            await _context.Entry(item).Reference(x => x.Slot).LoadAsync();
        }
        var response = _mapper.Map<List<AppointmentAdminResponse>>(apps);
        foreach (var item in response) {
            item.Creator = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(item.CreatorId));
            item.Mentor = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(item.MentorId));
            item.Slot.NumOfAppointments = await _context.Appointments
                                                        .Where(x => x.SlotId == item.SlotId
                                                                    && x.IsApprove == true)
                                                        .CountAsync();
        }
        return PagedResult<AppointmentAdminResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
    }

    public async Task<PagedResult<AppointmentUserResponse>> GetAllSendAppointmentsAsync(
        ClaimsPrincipal principal,
        AppointmentUserParameters param)
    {
        var result = new PagedResult<AppointmentUserResponse>();

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

        var apps = await _context.Appointments.Where(x => x.CreatorId == user.Id).ToListAsync();

        var query = apps.AsQueryable();

        FilterBySlot(ref query, param.SlotId);
        SortNewAppointment(ref query, param.IsNew);
        FilterActive(ref query, param.IsActive);
        FilterIsPassed(ref query, param.IsPassed);
        FilterApprove(ref query, param.IsApprove, param.IsAll);

        apps = query.ToList();
        foreach (var item in apps) {
            await _context.Entry(item).Reference(x => x.Slot).LoadAsync();
        }
        var response = _mapper.Map<List<AppointmentUserResponse>>(apps);
        foreach (var item in response) {
            item.Mentor = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(item.MentorId));
            item.Slot.NumOfAppointments = await _context.Appointments
                                                        .Where(x => x.SlotId == item.SlotId
                                                                    && x.IsApprove == true)
                                                        .CountAsync();
        }
        return PagedResult<AppointmentUserResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
    }

    private void FilterIsPassed(ref IQueryable<Core.Entities.Appointment> query, bool? isPassed)
    {
        if (!query.Any() || isPassed is null) {
            return;
        }
        query = query.Where(x => x.IsPassed == isPassed);
    }

    public async Task<Result<AppointmentAdminDetailResponse>> GetAppointmentByIdForAdminAsync(string appointmentId)
    {
        var result = new Result<AppointmentAdminDetailResponse>();

        var app = await _context.Appointments.FindAsync(appointmentId);
        if (app == null) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "subject.");
            return result;
        }
        await _context.Entry(app).Reference(x => x.Slot).Query().Include(x => x.SlotSubjects).ThenInclude(x => x.Subject).LoadAsync();
        await _context.Entry(app).Collection(x => x.Questions).LoadAsync();

        var response = _mapper.Map<AppointmentAdminDetailResponse>(app);

        response.Creator = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(response.CreatorId));
        response.Mentor = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(response.MentorId));
        response.Slot.NumOfAppointments = await _context.Appointments
                                                        .Where(x => x.SlotId == app.SlotId
                                                                    && x.IsApprove == true)
                                                        .CountAsync();

        result.Content = response;
        return result;
    }

    public async Task<Result<AppointmentMentorDetailResponse>> GetAppointmentReceiveByIdAsync(
        ClaimsPrincipal principal,
        string appointmentId)
    {
        var result = new Result<AppointmentMentorDetailResponse>();

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
        if (user.IsMentor is not true) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "You are not mentor!");
            return result;
        }

        var app = await _context.Appointments.FindAsync(appointmentId);
        if (app == null || app.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "subject.");
            return result;
        }
        if (app.MentorId != user.Id) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This is not your appointment!");
            return result;
        }
        await _context.Entry(app).Reference(x => x.Slot).Query().Include(x => x.SlotSubjects).ThenInclude(x => x.Subject).LoadAsync();
        await _context.Entry(app).Collection(x => x.Questions).LoadAsync();

        var response = _mapper.Map<AppointmentMentorDetailResponse>(app);

        response.Creator = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(response.CreatorId));
        response.Slot.NumOfAppointments = await _context.Appointments
                                                        .Where(x => x.SlotId == app.SlotId
                                                                    && x.IsApprove == true)
                                                        .CountAsync();

        result.Content = response;
        return result;
    }

    public async Task<Result<AppointmentUserDetailResponse>> GetAppointmentSendedByIdAsync(
        ClaimsPrincipal principal,
        string appointmentId)
    {
        var result = new Result<AppointmentUserDetailResponse>();

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

        var app = await _context.Appointments.FindAsync(appointmentId);
        if (app == null || app.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "subject.");
            return result;
        }
        if (app.CreatorId != user.Id) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This is not your appointment!");
            return result;
        }
        await _context.Entry(app).Reference(x => x.Slot).Query().Include(x => x.SlotSubjects).ThenInclude(x => x.Subject).LoadAsync();
        await _context.Entry(app).Collection(x => x.Questions).LoadAsync();

        var response = _mapper.Map<AppointmentUserDetailResponse>(app);

        response.Mentor = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(response.MentorId));
        response.Slot.NumOfAppointments = await _context.Appointments
                                                        .Where(x => x.SlotId == app.SlotId
                                                                    && x.IsApprove == true)
                                                        .CountAsync();

        result.Content = response;
        return result;
    }

    public async Task<Result<bool>> MentorUpdateAppointmentAsync(ClaimsPrincipal principal, string appointmentId, AppointmentUpdateRequest request)
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
        if (user.IsMentor is not true) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "You are not mentor!");
            return result;
        }

        var app = await _context.Appointments.FindAsync(appointmentId);
        if (app == null || app.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "subject.");
            return result;
        }
        if (app.MentorId != user.Id) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This is not your appointment!");
            return result;
        }

        var model = _mapper.Map(request, app);
        _context.Appointments.Update(model);

        if (await _context.SaveChangesAsync() >= 0) {
            result.Content = true;
            return result;
        }
        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                 ErrorTypes.SaveFail,
                                                 ErrorMessages.SaveFail);
        return result;
    }

    public async Task<Result<bool>> ProcessAppointmentAsync(ClaimsPrincipal principal, string appointmentId, AppointmentProcessRequest request)
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
        if (user.IsMentor is not true) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "You are not mentor!");
            return result;
        }

        var app = await _context.Appointments.FindAsync(appointmentId);
        if (app == null || app.IsActive == false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "subject.");
            return result;
        }
        if (app.MentorId != user.Id) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This is not your appointment!");
            return result;
        }

        if(app.IsApprove != null) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "This appointment has been processed!");
            return result;
        }

        if (request.IsApprove == false) {
            if (String.IsNullOrEmpty(request.MentorDescription) || String.IsNullOrWhiteSpace(request.MentorDescription)) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     "Please enter your description why you reject this appointment!");
                return result;
            }
            app.IsApprove = request.IsApprove;
            app.MentorDescription = request.MentorDescription;
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
            return result;
        }
        else {
            app.IsApprove = request.IsApprove;
            user.NumOfAppointment += 1;
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
            return result;
        }
    }

    public async Task<Result<bool>> CheckPassedAppointmentAsync()
    {
        var result = new Result<bool>();
        var now = DateTime.UtcNow.AddHours(7);

        var apps = await _context.Appointments.ToListAsync();
        foreach (var item in apps) {
            if (now > item.FinishTime) {
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
