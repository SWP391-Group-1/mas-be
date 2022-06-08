using AutoMapper;
using MAS.Core.Dtos.Incoming.Appointment;
using MAS.Core.Dtos.Outcoming.Appointment;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Interfaces.Repositories.Appointment;
using MAS.Core.Parameters.Appointment;
using MAS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
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

        public async Task<Result<bool>> CreateAppointmentAsync(ClaimsPrincipal principal, string mentorId, AppointmentCreateRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<bool>> DeleteAppointmentAsync(string appointmentId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedResult<AppointmentMentorResponse>> GetAllAppointmentsOfMentorAsync(ClaimsPrincipal principal, AppointmentMentorParameters param)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedResult<AppointmentAdminResponse>> GetAllAppointmentsOfMentorForAdminAsync(string mentorId, AppointmentAdminParameters param)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedResult<AppointmentUserResponse>> GetAllAppointmentsOfOwnAsync(ClaimsPrincipal principal, AppointmentUserParameters param)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<AppointmentAdminDetailResponse>> GetAppointmentByIdForAdminAsync(string appointmentId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<AppointmentMentorDetailResponse>> GetAppointmentOfMentorByIdAsync(ClaimsPrincipal principal, string appointmentId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<AppointmentUserDetailResponse>> GetAppointmentOfOwnByIdAsync(ClaimsPrincipal principal, string appointmentId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<bool>> MentorUpdateAppointmentAsync(ClaimsPrincipal principal, string appointmentId, AppointmentUpdateRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<bool>> ProcessAppointmentAsync(ClaimsPrincipal principal, string appointmentId, AppointmentProcessRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}