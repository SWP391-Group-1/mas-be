using AutoMapper;
using MAS.Core.Constants.Error;
using MAS.Core.Dtos.Incoming.Rating;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Rating;
using MAS.Core.Interfaces.Repositories.Rating;
using MAS.Core.Parameters.Rating;
using MAS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.Rating;
public class RatingRepository : BaseRepository, IRatingRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    public RatingRepository(IMapper mapper,
                              AppDbContext context,
                              UserManager<IdentityUser> userManager) : base(mapper, context)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> CreateRatingFeedbackAsync(
        ClaimsPrincipal principal,
        string appointmentId,
        CreateRatingRequest request)
    {
        var result = new Result<bool>();
        var loggedInUser = await _userManager.GetUserAsync(principal);
        if (loggedInUser is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, ErrorMessages.NotLogIn);
            return result;
        }
        var identityId = loggedInUser.Id;

        var user = await _context.MasUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
        if (user is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, ErrorMessages.NotFound);
            return result;
        }

        var appointment = await _context.Appointments.FindAsync(appointmentId);

        if (appointment is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;
        }
        if (appointment.IsApprove is false) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.BadRequest, "Not allow to rating.");
            return result;
        }
        if (appointment.CreatorId != user.Id) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, "Only owner can rating.");
            return result;
        }
        await _context.Entry(appointment).Collection(x => x.Ratings).LoadAsync();
        if (appointment.Ratings.Count >= 1) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, "You have already vote.");
            return result;
        }

        var toUser = await _context.MasUsers.FindAsync(appointment.MentorId);

        var model = _mapper.Map<Core.Entities.Rating>(request);
        model.AppointmentId = appointmentId;
        model.CreatorId = appointment.CreatorId;
        model.MentorId = appointment.MentorId;
        model.IsActive = false;
        model.IsApprove = null;

        await _context.Ratings.AddAsync(model);


        if ((await _context.SaveChangesAsync() >= 0)) {
            // var rateOfMentor = await _context.Ratings.Where(x => x.IsActive == true && x.MentorId == appointment.MentorId).AverageAsync(x => x.Vote);

            // user.Rate = (float)rateOfMentor;

            // if ((await _context.SaveChangesAsync() < 0)) {
            //     result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);
            //     return result;
            // }
            result.Content = true;
            return result;
        }
        result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);
        return result;

    }

    public async Task<PagedResult<RatingResponse>> GetAllRatingsAsync(string mentorId, RatingParameters param)
    {
        var result = new PagedResult<RatingResponse>();
        var mentor = await _context.MasUsers.FindAsync(mentorId);
        if (mentor is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, ErrorMessages.NotFound);
            return result;
        }

        if (mentor.IsActive is false) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, ErrorMessages.NotFound);
            return result;
        }

        var ratings = await _context.Ratings.Where(x => x.MentorId == mentorId).ToListAsync();
        var query = ratings.AsQueryable();

        FilterActiveRating(ref query, true);
        OrderByCreatedDate(ref query, param.IsNew);

        ratings = query.ToList();

        var response = _mapper.Map<List<RatingResponse>>(ratings);
        foreach (var item in response) {
            var creator = await _context.MasUsers.FindAsync(item.CreatorId);
            item.CreatorName = creator.Name;
            item.CreatorMail = creator.Email;
        }
        return PagedResult<RatingResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
    }

    private void FilterActiveRating(ref IQueryable<Core.Entities.Rating> query, bool? isActive)
    {
        if (!query.Any() || isActive is null) {
            return;
        }
        if (isActive is true) {
            query = query.Where(x => x.IsActive == true);
        }
        if (isActive is false) {
            query = query.Where(x => x.IsActive == false);
        }
    }

    private void OrderByCreatedDate(ref IQueryable<Core.Entities.Rating> query, bool? isNew)
    {
        if (!query.Any() || isNew is null) {
            return;
        }
        if (isNew is true) {
            query = query.OrderByDescending(x => x.CreateDate);
        }
        if (isNew is false) {
            query = query.OrderBy(x => x.CreateDate);
        }
    }
    private void FilterApproveRating(ref IQueryable<Core.Entities.Rating> query, bool? isApprove)
    {
        if (!query.Any()) {
            return;
        }
        query = query.Where(x => x.IsApprove == isApprove);
    }

    public async Task<PagedResult<RatingResponse>> GetAllRatingsForAdminAsync(RatingParametersAdmin param)
    {
        var ratings = await _context.Ratings.ToListAsync();
        var query = ratings.AsQueryable();

        FilterActiveRating(ref query, param.IsActive);
        FilterApproveRating(ref query, param.IsApprove);
        OrderByCreatedDate(ref query, param.IsNew);

        ratings = query.ToList();

        var response = _mapper.Map<List<RatingResponse>>(ratings);
        foreach (var item in response) {
            var creator = await _context.MasUsers.FindAsync(item.CreatorId);
            item.CreatorName = creator.Name;
            item.CreatorMail = creator.Email;
        }
        return PagedResult<RatingResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
    }

    public async Task<Result<RatingResponse>> GetRatingByIdAsync(string ratingId)
    {
        var result = new Result<RatingResponse>();
        var rating = await _context.Ratings.FindAsync(ratingId);
        if (rating is null || (rating.IsActive is false && rating.IsApprove is not null)) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;
        }
        var response = _mapper.Map<RatingResponse>(rating);
        var creator = await _context.MasUsers.FindAsync(response.CreatorId);
        response.CreatorName = creator.Name;
        response.CreatorMail = creator.Email;
        result.Content = response;
        return result;
    }

    public async Task<Result<bool>> ProcessRatingAsync(string ratingId, ProcessRatingRequest request)
    {
        var result = new Result<bool>();
        var rating = await _context.Ratings.FindAsync(ratingId);
        if (rating is null) {
            result.Error = Helpers.ErrorHelper.PopulateError(404, ErrorTypes.NotFound, ErrorMessages.NotFound);
            return result;
        }
        if (rating.IsApprove is not null) {
            result.Error = Helpers.ErrorHelper.PopulateError(400, ErrorTypes.BadRequest, $"This rating has been processed.");
            return result;
        }

        if (request.IsApprove is false) {
            rating.IsApprove = false;
            rating.IsActive = false;
        }
        else {
            var mentor = await _context.MasUsers.FindAsync(rating.MentorId);
            rating.IsActive = true;
            rating.IsApprove = true;
            mentor.NumOfRate += 1;

            if ((await _context.SaveChangesAsync() >= 0)) {
                if (await _context.Ratings.AnyAsync(x => x.IsActive == true && x.MentorId == rating.MentorId)) {
                    var rateOfMentor = _context.Ratings.Where(x => x.IsActive == true && x.MentorId == rating.MentorId).Average(x => x.Vote);
                    mentor.Rate = (float)rateOfMentor;
                }
                if (await _context.SaveChangesAsync() >= 0) {
                    result.Content = true;
                    return result;
                }
            }
        }
        result.Error = Helpers.ErrorHelper.PopulateError(0, ErrorTypes.SaveFail, ErrorMessages.SaveFail);
        return result;
    }
}
