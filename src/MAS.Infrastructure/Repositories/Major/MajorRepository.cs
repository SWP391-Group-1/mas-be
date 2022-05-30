using AutoMapper;
using MAS.Core.Constants.Error;
using MAS.Core.Dtos.Incoming.Major;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Major;
using MAS.Core.Interfaces.Repositories.Major;
using MAS.Core.Parameters.Major;
using MAS.Infrastructure.Data;
using MAS.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.Major
{
    public class MajorRepository : BaseRepository, IMajorRepository
    {
        public MajorRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<Result<MajorResponse>> CreateMajorAsync(MajorCreateRequest request)
        {
            var result = new Result<MajorResponse>();

            var existMajor = await _context.Majors.AnyAsync(x => x.Title.ToLower().Trim() == request.Title.ToLower().Trim());
            if (existMajor) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.Exist + $"{request.Title} in System.");
                return result;
            }

            var model = _mapper.Map<Core.Entities.Major>(request);
            await _context.Majors.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                var response = _mapper.Map<MajorResponse>(model);
                result.Content = response;
                return result;
            }
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
            return result;
        }

        public async Task<Result<bool>> DeleteMajorAsync(string MajorId)
        {
            var result = new Result<bool>();

            var Major = await _context.Majors.FindAsync(MajorId);
            if (Major == null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                         ErrorTypes.NotFound,
                                                         ErrorMessages.NotFound + "Major.");
                return result;
            }

            _context.Majors.Remove(Major);
            if ((await _context.SaveChangesAsync() >= 0)) {
                result.Content = true;
                return result;
            }
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.Else,
                                                     ErrorTypes.SaveFail,
                                                     ErrorMessages.SaveFail);
            return result;
        }

        public async Task<PagedResult<MajorResponse>> GetAllMajorsAsync(MajorParameters param)
        {
            var Majors = await _context.Majors.ToListAsync();
            var query = Majors.AsQueryable();

            FilterMajor(ref query, param.SearchString);

            Majors = query.ToList();
            var response = _mapper.Map<List<MajorResponse>>(Majors);
            return PagedResult<MajorResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterMajor(
            ref IQueryable<Core.Entities.Major> query,
            string searchString)
        {
            if (!query.Any()
               || String.IsNullOrEmpty(searchString)
               || String.IsNullOrWhiteSpace(searchString)) {
                return;
            }
            query = query.Where(x => (x.Title + " " + x.Description)
                                    .ToLower()
                                    .Contains(searchString.ToLower())
                               );
        }

        public async Task<Result<MajorResponse>> GetMajorByIdAsync(string MajorId)
        {
            var result = new Result<MajorResponse>();

            var Major = await _context.Majors.FindAsync(MajorId);
            if (Major == null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                         ErrorTypes.NotFound,
                                                         ErrorMessages.NotFound + "Major.");
                return result;
            }

            var response = _mapper.Map<MajorResponse>(Major);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> UpdateMajorAsync(string MajorId, MajorUpdateRequest request)
        {
            var result = new Result<bool>();

            var Major = await _context.Majors.FindAsync(MajorId);
            if (Major == null) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                         ErrorTypes.NotFound,
                                                         ErrorMessages.NotFound + "Major.");
                return result;
            }

            var existMajor = await _context.Majors.AnyAsync(x => x.Title.ToLower().Trim() == request.Title.ToLower().Trim()
                                                                    && x.Id != MajorId);
            if (existMajor) {
                result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                         ErrorTypes.BadRequest,
                                                         ErrorMessages.Exist + $"{request.Title} in System.");
                return result;
            }

            var model = _mapper.Map(request, Major);
            _context.Majors.Update(model);

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
