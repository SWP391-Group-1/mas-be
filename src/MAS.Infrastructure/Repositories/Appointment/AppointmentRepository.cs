using System;
using AutoMapper;
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
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.Appointment
{
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
            if (slot is null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                         ErrorTypes.NotFound,
                                                         ErrorMessages.NotFound + "Slot");
                return result;
            }

            if (slot.StartTime.AddDays(-1) < request.CreateDate) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                         ErrorTypes.NotFound,
                                                         "Not allow to register appointment in this slot!. You can register a slot before it start at least 1 day!");
                return result;
            }

            foreach (var item in request.AppointmentSubjects) {
                var subject = await _context.Subjects.FindAsync(item.SubjectId);
                if (subject is null) {
                    result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                         ErrorTypes.NotFound,
                                                         ErrorMessages.NotFound + "Subject");
                    return result;
                }
            }

            await _context.Appointments.AddAsync(
                new Core.Entities.Appointment {
                    Id = request.Id,
                    CreateDate = request.CreateDate,
                    UpdateDate = null,
                    CreatorId = user.Id,
                    MentorId = slot.MentorId,
                    SlotId = request.SlotId,
                    IsApprove = null,
                    StartTime = null,
                    FinishTime = null,
                    MentorDescription = ""
                }
            );
            if ((await _context.SaveChangesAsync() >= 0)) {
                foreach (var item in request.AppointmentSubjects) {
                    await _context.AppointmentSubjects.AddAsync(
                        new Core.Entities.AppointmentSubject {
                            Id = item.Id,
                            CreateDate = item.CreateDate,
                            UpdateDate = null,
                            AppointmentId = request.Id,
                            SubjectId = item.SubjectId,
                            BriefProblem = item.BriefProblem
                        }
                    );
                    if ((await _context.SaveChangesAsync() < 0)) {
                        result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
                        return result;
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
            if (appointment is null) {
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

            await _context.Entry(appointment).Collection(x => x.AppointmentSubjects).LoadAsync();
            _context.AppointmentSubjects.RemoveRange(appointment.AppointmentSubjects);
            if ((await _context.SaveChangesAsync() >= 0)) {
                _context.Appointments.Remove(appointment);
                if ((await _context.SaveChangesAsync() >= 0)) {
                    result.Content = true;
                    return result;
                }
            }
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
            return result;
        }

        public async Task<PagedResult<AppointmentMentorResponse>> GetAllAppointmentsOfMentorAsync(
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

            apps = query.ToList();
            foreach (var item in apps) {
                await _context.Entry(item).Reference(x => x.Slot).LoadAsync();
            }
            var response = _mapper.Map<List<AppointmentMentorResponse>>(apps);
            foreach (var item in response) {
                item.Creator = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(item.CreatorId));
            }
            return PagedResult<AppointmentMentorResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
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

        public async Task<PagedResult<AppointmentAdminResponse>> GetAllAppointmentsOfMentorForAdminAsync(
            string mentorId,
            AppointmentAdminParameters param)
        {
            var apps = await _context.Appointments.Where(x => x.MentorId == mentorId).ToListAsync();

            var query = apps.AsQueryable();

            FilterBySlot(ref query, param.SlotId);
            SortNewAppointment(ref query, param.IsNew);

            apps = query.ToList();
            foreach (var item in apps) {
                await _context.Entry(item).Reference(x => x.Slot).LoadAsync();
            }
            var response = _mapper.Map<List<AppointmentAdminResponse>>(apps);
            foreach (var item in response) {
                item.Creator = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(item.CreatorId));
                item.Mentor = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(item.MentorId));
            }
            return PagedResult<AppointmentAdminResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        public async Task<PagedResult<AppointmentUserResponse>> GetAllAppointmentsOfOwnAsync(
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

            apps = query.ToList();
            foreach (var item in apps) {
                await _context.Entry(item).Reference(x => x.Slot).LoadAsync();
            }
            var response = _mapper.Map<List<AppointmentUserResponse>>(apps);
            foreach (var item in response) {
                item.Mentor = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(item.MentorId));
            }
            return PagedResult<AppointmentUserResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
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
            await _context.Entry(app).Reference(x => x.Slot).LoadAsync();
            await _context.Entry(app).Collection(x => x.Questions).LoadAsync();
            await _context.Entry(app).Collection(x => x.AppointmentSubjects).LoadAsync();

            var response = _mapper.Map<AppointmentAdminDetailResponse>(app);

            response.Creator = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(response.CreatorId));
            response.Mentor = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(response.MentorId));

            result.Content = response;
            return result;
        }

        public async Task<Result<AppointmentMentorDetailResponse>> GetAppointmentOfMentorByIdAsync(
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
            if (app == null) {
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
            await _context.Entry(app).Reference(x => x.Slot).LoadAsync();
            await _context.Entry(app).Collection(x => x.Questions).LoadAsync();
            await _context.Entry(app).Collection(x => x.AppointmentSubjects).LoadAsync();

            var response = _mapper.Map<AppointmentMentorDetailResponse>(app);

            response.Creator = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(response.CreatorId));

            result.Content = response;
            return result;
        }

        public async Task<Result<AppointmentUserDetailResponse>> GetAppointmentOfOwnByIdAsync(
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
            if (app == null) {
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
            await _context.Entry(app).Reference(x => x.Slot).LoadAsync();
            await _context.Entry(app).Collection(x => x.Questions).LoadAsync();
            await _context.Entry(app).Collection(x => x.AppointmentSubjects).LoadAsync();

            var response = _mapper.Map<AppointmentUserDetailResponse>(app);

            response.Mentor = UserHelper.PopulateUser(await _context.MasUsers.FindAsync(response.MentorId));

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
            if (app == null) {
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
            if (app == null) {
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
    }
}