﻿using AutoMapper;
using MAS.Core.Constants.Error;
using MAS.Core.Dtos.Incoming.MentorSubject;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.MentorSubject;
using MAS.Core.Interfaces.Repositories.MentorSubject;
using MAS.Core.Parameters.MentorSubject;
using MAS.Infrastructure.Data;
using MAS.Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.MentorSubject
{
    public class MentorSubjectRepository : BaseRepository, IMentorSubjectRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public MentorSubjectRepository(IMapper mapper,
                                       AppDbContext context,
                                       UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> DeleteSubjectOfMentorAsync(
            ClaimsPrincipal principal,
            string subjectOfMentorId)
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

            var mentorSubject = await _context.MentorSubjects.FindAsync(subjectOfMentorId);
            if (mentorSubject is null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.NotFound + "this information!");
                return result;
            }

            if (mentorSubject.MentorId != user.Id) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.NotAllowModify);
                return result;
            }

            _context.MentorSubjects.Remove(mentorSubject);
            if ((await _context.SaveChangesAsync() >= 0)) {
                result.Content = true;
                return result;
            }
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
            return result;
        }

        public async Task<PagedResult<MentorSubjectResponse>> GetAllsSubjectOfMentorAsync(
            string mentorId,
            MentorSubjectParameters param)
        {
            var result = new PagedResult<MentorSubjectResponse>();

            var mentor = await _context.MasUsers.FindAsync(mentorId);
            if (mentor == null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.NotFound + "mentor!");
                return result;
            }
            if (mentor.IsActive is false) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         "This mentor account is unactive!");
                return result;
            }

            var mentorSubjects = await _context.MentorSubjects.Where(x => x.MentorId == mentorId).ToListAsync();
            foreach (var mentorSubject in mentorSubjects) {
                await _context.Entry(mentorSubject).Reference(x => x.Subject).LoadAsync();
            }
            var response = _mapper.Map<List<MentorSubjectResponse>>(mentorSubjects);
            return PagedResult<MentorSubjectResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        public async Task<Result<bool>> RegisterSubjectAsync(
            ClaimsPrincipal principal,
            MentorSubjectRegisterRequest request)
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

            var model = _mapper.Map<Core.Entities.MentorSubject>(request);
            model.MentorId = user.Id;
            await _context.MentorSubjects.AddAsync(model);

            if ((await _context.SaveChangesAsync() >= 0)) {
                result.Content = true;
                return result;
            }
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
            return result;
        }

        public async Task<Result<bool>> UpdateSubjectOfMentorAsync(
            ClaimsPrincipal principal,
            string subjectOfMentorId,
            MentorSubjectUpdateRequest request)
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

            var mentorSubject = await _context.MentorSubjects.FindAsync(subjectOfMentorId);
            if (mentorSubject is null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.NotFound + "this information!");
                return result;
            }

            if (mentorSubject.MentorId != user.Id) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.NotAllowModify);
                return result;
            }

            var subject = await _context.Subjects.FindAsync(request.SubjectId);
            if (subject is null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.NotFound + "this subject!");
                return result;
            }

            var model = _mapper.Map(request, mentorSubject);
            _context.MentorSubjects.Update(model);

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
