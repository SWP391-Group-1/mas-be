using AutoMapper;
using MAS.Core.Constants.Error;
using MAS.Core.Dtos.Incoming.Subject;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Subject;
using MAS.Core.Interfaces.Repositories.Subject;
using MAS.Core.Parameters.Subject;
using MAS.Infrastructure.Data;
using MAS.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.Subject
{
    public class SubjectRepository : BaseRepository, ISubjectRepository
    {
        public SubjectRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<Result<SubjectResponse>> CreateSubjectAsync(SubjectCreateRequest request)
        {
            var result = new Result<SubjectResponse>();

            var existSubject = await _context.Subjects.AnyAsync(x => x.Title.ToLower().Trim() == request.Title.ToLower().Trim());
            if (existSubject) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.Exist + $"{request.Title} in System.");
                return result;
            }

            var model = _mapper.Map<Core.Entities.Subject>(request);
            await _context.Subjects.AddAsync(model);

            if ((await _context.SaveChangesAsync() >= 0)) {
                var response = _mapper.Map<SubjectResponse>(model);
                result.Content = response;
                return result;
            }
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
            return result;
        }

        public async Task<Result<bool>> DeleteSubjectAsync(string subjectId)
        {
            var result = new Result<bool>();

            var subject = await _context.Subjects.FindAsync(subjectId);
            if (subject == null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                         ErrorTypes.NotFound,
                                                         ErrorMessages.NotFound + "subject.");
                return result;
            }

            var appSubjects = await _context.AppointmentSubjects.Where(x => x.SubjectId == subjectId).ToListAsync();
            if (appSubjects.Count > 0) {
                _context.AppointmentSubjects.RemoveRange(appSubjects);
                if(await _context.SaveChangesAsync() < 0) {
                    result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else, 
                                                             ErrorTypes.SaveFail,
                                                             ErrorMessages.SaveFail);
                    return result;
                }
            }

            var mentorSubjects = await _context.MentorSubjects.Where(x => x.SubjectId == subjectId).ToListAsync();
            if(mentorSubjects.Count > 0) {
                _context.MentorSubjects.RemoveRange(mentorSubjects);
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                             ErrorTypes.SaveFail,
                                                             ErrorMessages.SaveFail);
                    return result;
                }
            }

            _context.Subjects.Remove(subject);
            if ((await _context.SaveChangesAsync() >= 0)) {
                result.Content = true;
                return result;
            }
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
            return result;
        }

        public async Task<PagedResult<SubjectResponse>> GetAllSubjectsAsync(SubjectParameters param)
        {
            var subjects = await _context.Subjects.ToListAsync();
            var query = subjects.AsQueryable();

            FilterSubject(ref query, param.SearchString);

            subjects = query.ToList();
            var response = _mapper.Map<List<SubjectResponse>>(subjects);
            return PagedResult<SubjectResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterSubject(
            ref IQueryable<Core.Entities.Subject> query,
            string searchString)
        {
            if(!query.Any()
               || String.IsNullOrEmpty(searchString)
               || String.IsNullOrWhiteSpace(searchString)) {
                return;
            }
            query = query.Where(x => 
                                    (x.Title + " " + x.Description)
                                    .ToLower()
                                    .Contains(searchString.ToLower())
                               );
        }

        public async Task<Result<SubjectResponse>> GetSubjectByIdAsync(string subjectId)
        {
            var result = new Result<SubjectResponse>();
            
            var subject = await _context.Subjects.FindAsync(subjectId);
            if(subject == null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                         ErrorTypes.NotFound,
                                                         ErrorMessages.NotFound + "subject.");
                return result;
            }

            var response = _mapper.Map<SubjectResponse>(subject);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> UpdateSubjectAsync(string subjectId, SubjectUpdateRequest request)
        {
            var result = new Result<bool>();

            var subject = await _context.Subjects.FindAsync(subjectId);
            if (subject == null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                         ErrorTypes.NotFound,
                                                         ErrorMessages.NotFound + "subject.");
                return result;
            }

            var existSubject = await _context.Subjects.AnyAsync(x => x.Title.ToLower().Trim() == request.Title.ToLower().Trim() 
                                                                    && x.Id != subjectId);
            if (existSubject) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.Exist + $"{request.Title} in System.");
                return result;
            }

            var model = _mapper.Map(request, subject);
            _context.Subjects.Update(model);

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
