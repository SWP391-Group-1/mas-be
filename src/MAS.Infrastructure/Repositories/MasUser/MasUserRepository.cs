using AutoMapper;
using MAS.Core.Constants.Error;
using MAS.Core.Dtos.Incoming.MasUser;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.MasUser;
using MAS.Core.Interfaces.Repositories.MasUser;
using MAS.Core.Parameters.MasUser;
using MAS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.MasUser;

public class MasUserRepository : BaseRepository, IMasUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    public MasUserRepository
        (IMapper mapper,
        AppDbContext context,
        UserManager<IdentityUser> userManager) : base(mapper, context)
    {
        _userManager = userManager;
    }


    public async Task<Result<bool>> ChangeIsActiveUserForAdminAsync(string userId, IsActiveChangeRequest request)
    {
        var result = new Result<bool>();
        var user = await _context.MasUsers.FindAsync(userId);

        if (user is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;

        }

        if (request.IsActive == true) {
            if (user.IsActive == true) {
                result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, $"Account {user.Name} is actived.");
                return result;
            }
            user.IsActive = true;
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }

            result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);
            return result;
        }
        else {
            if (user.IsActive == false) {
                result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, $"Account {user.Name} is locked.");
                return result;
            }
            if (String.IsNullOrEmpty(request.Note) || String.IsNullOrWhiteSpace(request.Note)) {
                result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, $"Reason is required.");
                return result;
            }

            if (await _context.SaveChangesAsync() >= 0) {
                user.IsActive = false;

                //    await _context.Notifications.AddAsync(
                //            Helpers.NotificationHelpers.PopulateNotification(
                //            userId, $"Xin chào {user.Name}, bạn đã bị khóa tài khoản {request.NumDateDisable} ngày!!!",
                //            $"Vì: {request.Note}. Tài khoản của bạn sẽ mở lại lúc {request.DateActive}!!!. Bắt đầu khóa lúc {request.DateDisable}. Chào bạn!!!",
                //            ""
                //        )
                //    );
                if (await _context.SaveChangesAsync() >= 0) {
                    result.Content = true;
                    return result;
                }
            }
            result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);
            return result;

        }
    }

    public async Task<PagedResult<UserSearchResponse>> GetAllMentorsAsync(ClaimsPrincipal principal, UserParameters param)
    {
        var result = new PagedResult<UserSearchResponse>();
        var loggedInUser = await _userManager.GetUserAsync(principal);
        if (loggedInUser is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, ErrorMessages.NotAllowModify);
            return result;
        }
        var identityId = loggedInUser.Id;

        var user = await _context.MasUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

        if (user is null || user.IsActive is false) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;
        }

        var users = await _context.MasUsers.ToListAsync();

        var query = users.AsQueryable();

        Search(ref query, user.Id, param.Search);

        FilterUserBySubjectId(ref query, param.SubjectId);

        // FilterUserByDate(ref query, param.DayInWeek);

        FilterIsMentor(ref query, param.IsMentor);
        FilterActiveUser(ref query, true);

        OrderUserByASCName(ref query, param.IsOrderByName);

        users = query.ToList();

        var response = _mapper.Map<List<UserSearchResponse>>(users);
        //foreach (var item in response) {
        //    var rates = await _context.Ratings.Where(x => x.MentorId == item.Id).ToListAsync();
        //    item.NumOfRate = rates.Count();
        //}
        return result = PagedResult<UserSearchResponse>
            .ToPagedList(
                response,
                param.PageNumber,
                param.PageSize);
    }


    private void FilterUserByHour(ref IQueryable<Core.Entities.MasUser> query, string fromHour, string toHour)
    {
        if (!query.Any()
           || String.IsNullOrEmpty(fromHour)
           || String.IsNullOrWhiteSpace(fromHour)
           || String.IsNullOrEmpty(toHour)
           || String.IsNullOrWhiteSpace(toHour)) {
            return;
        }

        int fH = Int32.Parse(fromHour);
        int tH = Int32.Parse(toHour);
        if (fH > tH) {
            return;
        }
        var list = new List<Core.Entities.MasUser>();
        var datings = _context.Slots.Where(x => x.StartTime.Hour >= fH && x.FinishTime.Hour >= tH);
        foreach (var item in datings) {
            var user = _context.MasUsers.Find(item.MentorId);
            list.Add(user);
        }
        query = list.AsQueryable();
    }
    private void OrderUserByASCName(ref IQueryable<Core.Entities.MasUser> query, bool? isOrderByName)
    {
        if (!query.Any() || isOrderByName is null) {
            return;
        }
        if (isOrderByName == true) {
            query = query.OrderBy(x => x.Name);
        }
        else {
            query = query.OrderByDescending(x => x.Name);
        }
    }
    private void OrderUserByCreatedDate(ref IQueryable<Core.Entities.MasUser> query, bool? isNewAccount)
    {
        if (!query.Any() || isNewAccount is null) {
            return;
        }
        if (isNewAccount is true) {
            query = query.OrderByDescending(x => x.CreateDate);
        }
        else {
            query = query.OrderBy(x => x.CreateDate);
        }
    }
    private void FilterUserBySubjectId(
        ref IQueryable<Core.Entities.MasUser> query,
        string subjectId)
    {
        if (!query.Any() || String.IsNullOrEmpty(subjectId) || String.IsNullOrWhiteSpace(subjectId)) {
            return;
        }
        if (subjectId.Contains(" ")) {
            var seperatringSubjectId = subjectId.Split(" ");
            List<Core.Entities.MasUser> result = new();
            foreach (var sprId in seperatringSubjectId) {
                var subjectsOfUser = _context.MentorSubjects.Where(x => x.SubjectId == sprId);
                foreach (var item in subjectsOfUser) {
                    result.Add(item.Mentor);
                }
            }
            query = result.Distinct().AsQueryable();
        }
        else if (subjectId.Contains(",")) {
            var seperatringSubjectId = subjectId.Split(",");
            List<Core.Entities.MasUser> result = new();
            foreach (var sprId in seperatringSubjectId) {
                var subjectsOfUser = _context.MentorSubjects.Where(x => x.SubjectId == sprId);
                foreach (var item in subjectsOfUser) {
                    result.Add(item.Mentor);
                }
            }
            query = result.Distinct().AsQueryable();
        }
        else {
            List<Core.Entities.MasUser> result = new();
            var subjectsOfUser = _context.MentorSubjects.Where(x => x.SubjectId == subjectId);
            foreach (var item in subjectsOfUser) {
                result.Add(item.Mentor);
            }
            query = result.AsQueryable();
        }

    }
    private void FilterUserByNameVsEmail(
        ref IQueryable<Core.Entities.MasUser> query,
        string name)
    {
        if (!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
            return;
        }
        query = query.Where(x => (x.Name.ToLower() + " " + x.Email.ToLower()).Contains(name.ToLower()));
    }


    private void FilterIsMentor(ref IQueryable<Core.Entities.MasUser> query, bool? isMentor)
    {
        if (!query.Any() || isMentor is null) {
            return;
        }
        query = query.Where(x => x.IsMentor == isMentor);
    }

    private void FilterActiveUser(ref IQueryable<Core.Entities.MasUser> query, bool? isActive)
    {
        if (!query.Any() || isActive is null) {
            return;
        }
        query = query.Where(x => x.IsActive == isActive);
    }
    private void Search(ref IQueryable<Core.Entities.MasUser> query, string userId, string searchString)
    {
        if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString)) {
            return;
        }

        //var existSearch = _context.SearchHistories.Where(x => x.UserId == userId).Any(x => x.SearchString.ToLower() == searchString.ToLower());
        //if (existSearch is true) {
        //    var search = _context.SearchHistories.FirstOrDefault(x => x.UserId == userId && x.SearchString.ToLower() == searchString.ToLower());
        //    if (search is not null) {
        //        search.UpdateDate = DateTime.UtcNow.AddHours(7);
        //        search.IsActive = true;
        //    }
        //}
        //else {
        //    _context.SearchHistories.Add(Helpers.SearchHistoryHelpers.PopulateSearchHistory(userId, searchString));
        //}

        if (_context.SaveChanges() < 0) return;

        query = query.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
        if (query.Any()) {
            return;
        }
        else {
            var finalList = new List<Core.Entities.MasUser>();

            var seperateSearchStrings = searchString.Split(" ");
            if (searchString.Contains(" ")) {

            }
            else if (searchString.Contains("_")) {
                seperateSearchStrings = searchString.Split("_");
            }
            else if (searchString.Contains("-")) {
                seperateSearchStrings = searchString.Split("-");
            }

            foreach (var item in seperateSearchStrings) {
                finalList = finalList.Union(_context.MasUsers.Where(x => x.Name.ToLower().Contains(item.ToLower()))
                                                             .ToList()).ToList();

                var listFromSubject = _context.Subjects.Where(x => (x.Title + x.Description).ToLower().Contains(item.ToLower()))
                                                 .ToList();

                var listMentorSubject = new List<Core.Entities.MentorSubject>();
                foreach (var subject in listFromSubject) {
                    var list = _context.MentorSubjects.Where(x => x.Id == subject.Id).ToList();
                    listMentorSubject = listMentorSubject.Union(list).ToList();
                }

                foreach (var mentorSubject in listMentorSubject) {
                    var list = _context.MasUsers.Where(x => x.Id == mentorSubject.MentorId).ToList();
                    finalList = finalList.Union(list).ToList();
                }
            }
            query = finalList.AsQueryable();
        }
    }

    public async Task<PagedResult<UserGetByAdminResponse>> GetAllUsersForAdminAsync(AdminUserParameters param)
    {
        var users = await _context.MasUsers.ToListAsync();
        var query = users.AsQueryable();

        FilterActiveUser(ref query, param.IsActive);
        FilterUserByNameVsEmail(ref query, param.Name);
        FilterIsMentor(ref query, param.IsMentor);
        OrderUserByCreatedDate(ref query, param.IsNew);

        users = query.ToList();
        var response = _mapper.Map<List<UserGetByAdminResponse>>(users);
        return PagedResult<UserGetByAdminResponse>
            .ToPagedList(
                response,
                param.PageNumber,
                param.PageSize);
    }

    public async Task<Result<PersonalInfoResponse>> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal)
    {
        var result = new Result<PersonalInfoResponse>();
        var loggedInUser = await _userManager.GetUserAsync(principal);
        if (loggedInUser is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, ErrorMessages.NotFound);
            return result;
        }
        var identityId = loggedInUser.Id;

        var user = await _context.MasUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
        if (user is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;
        }

        if (await _context.SaveChangesAsync() < 0) {
            result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);

            return result;
        }

        var response = _mapper.Map<PersonalInfoResponse>(user);
        result.Content = response;
        return result;
    }

    public async Task<Result<UserGetBasicInfoResponse>> GetUserBasicInfoByIdAsync(string userId)
    {
        var result = new Result<UserGetBasicInfoResponse>();
        var user = await _context.MasUsers.FindAsync(userId);
        if (user is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;
        }
        // await _context.Entry(user).Collection(x => x.Slots).Query().OrderBy(x => x.DayInWeek).ThenBy(x => x.FromHour).LoadAsync();

        //var rates = await _context.Ratings.Where(x => x.MentorId == user.Id).ToListAsync();

        //user.NumOfRate = rates.Count();


        if (await _context.SaveChangesAsync() < 0) {
            result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);
            return result;
        }

        var response = _mapper.Map<UserGetBasicInfoResponse>(user);
        result.Content = response;
        return result;
    }

    public async Task<Result<bool>> SendMentorRequest(ClaimsPrincipal principal)
    {
        var result = new Result<bool>();
        var loggedInUser = await _userManager.GetUserAsync(principal);
        if (loggedInUser is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, ErrorMessages.NotFound);
            return result;
        }
        var identityId = loggedInUser.Id;

        var user = await _context.MasUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
        if (user is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;
        }


        if (user.IsMentor == true) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, $"Account is already mentor.");
            return result;
        }

        if (user.IsMentor == false) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, $"Request is pending.");
            return result;
        }

        user.IsMentor = false;

        if (await _context.SaveChangesAsync() >= 0) {
            result.Content = true;
            return result;
        }

        result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);

        return result;
    }


    public async Task<Result<bool>> AcceptRequest(string userId, MentorRequest request)
    {
        var result = new Result<bool>();
        var user = await _context.MasUsers.FindAsync(userId);

        if (user is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;

        }

        if (request.IsMentor == true) {
            if (user.IsMentor == true) {
                result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, $"Account is already mentor.");
                return result;
            }
            user.IsMentor = true;
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }

            result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);
            return result;
        }
        else if (request.IsMentor == false) {
            if (user.IsMentor == false) {
                result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, $"Request is pending.");
                return result;
            }

        }
        else {
            user.IsMentor = null;
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
        }

        result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);

        return result;
    }


    public async Task<Result<bool>> UpdatePersonalInfoAsync(ClaimsPrincipal principal, UserPersonalInfoUpdateRequest request)
    {
        var result = new Result<bool>();
        var loggedInUser = await _userManager.GetUserAsync(principal);
        if (loggedInUser is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, ErrorMessages.NotAllowModify);
            return result;
        }
        var identityId = loggedInUser.Id;
        var user = await _context.MasUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
        if (user is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;
        }

        _mapper.Map(request, user);
        _context.MasUsers.Update(user);
        if (await _context.SaveChangesAsync() >= 0) {
            result.Content = true;
            return result;
        }
        result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);
        return result;
    }
}
