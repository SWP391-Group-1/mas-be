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

namespace MAS.Infrastructure.Repositories.Major;

public class MajorRepository : BaseRepository, IMajorRepository
{
    public MajorRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
    {
    }

    public async Task<Result<MajorResponse>> CreateMajorAsync(MajorCreateRequest request)
    {
        var result = new Result<MajorResponse>();

        var existCodeMajor = await _context.Majors.AnyAsync(x => (x.Code.ToLower().Trim() == request.Code.ToLower().Trim()
                                                                    && x.IsActive == true));
        if (existCodeMajor) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.Exist + $"{request.Code} in System.");
            return result;
        }

        var existTitleMajor = await _context.Majors.AnyAsync(x => (x.Title.ToLower().Trim() == request.Title.ToLower().Trim()
                                                                    && x.IsActive == true));
        if (existTitleMajor) {
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

        var major = await _context.Majors.FindAsync(MajorId);
        if (major == null || major.IsActive == false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "Major.");
            return result;
        }

        var subjectInMajor = await _context.Subjects.Where(x => x.MajorId == major.Id).CountAsync();
        if (subjectInMajor > 0) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     "Have subjects in this major. Please change or modify these information.");
            return result;
        }

        major.IsActive = false;
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
        var majors = await _context.Majors.ToListAsync();
        var query = majors.AsQueryable();

        FilterMajor(ref query, param.SearchString);
        FilterActive(ref query, param.IsActive);

        majors = query.ToList();
        var response = _mapper.Map<List<MajorResponse>>(majors);
        return PagedResult<MajorResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
    }

    private void FilterActive(ref IQueryable<Core.Entities.Major> query, bool? isActive)
    {
        if (!query.Any() || isActive is null) {
            return;
        }
        query = query.Where(x => x.IsActive == isActive);
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
        query = query.Where(x => (x.Title + " " + x.Description + " " + x.Code)
                                .ToLower()
                                .Contains(searchString.ToLower())
                           );
    }

    public async Task<Result<MajorResponse>> GetMajorByIdAsync(string majorId)
    {
        var result = new Result<MajorResponse>();

        var major = await _context.Majors.FindAsync(majorId);
        if (major == null || major.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "Major.");
            return result;
        }

        var response = _mapper.Map<MajorResponse>(major);
        result.Content = response;
        return result;
    }

    public async Task<Result<bool>> UpdateMajorAsync(string majorId, MajorUpdateRequest request)
    {
        var result = new Result<bool>();

        var major = await _context.Majors.FindAsync(majorId);
        if (major == null || major.IsActive is false) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.NotFound,
                                                     ErrorTypes.NotFound,
                                                     ErrorMessages.NotFound + "Major.");
            return result;
        }

        var existCodeMajor = await _context.Majors.AnyAsync(x => (x.Code.ToLower().Trim() == request.Code.ToLower().Trim()
                                                                && x.Id != majorId && x.IsActive == true));
        if (existCodeMajor) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.Exist + $"{request.Code} in System.");
            return result;
        }

        var existTitleMajor = await _context.Majors.AnyAsync(x => (x.Title.ToLower().Trim() == request.Title.ToLower().Trim()
                                                                && x.Id != majorId && x.IsActive == true));
        if (existTitleMajor) {
            result.Error = ErrorHelper.PopulateError((int)ErrorCodes.BadRequest,
                                                     ErrorTypes.BadRequest,
                                                     ErrorMessages.Exist + $"{request.Title} in System.");
            return result;
        }

        var model = _mapper.Map(request, major);
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
