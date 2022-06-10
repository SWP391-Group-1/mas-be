using MAS.Core.Dtos.Incoming.Appointment;
using MAS.Core.Dtos.Outcoming.Appointment;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Parameters.Appointment;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.Appointment;

public interface IAppointmentRepository
{
    /* Student create, delete appointment, view all appointments and view in detail appointment */
    Task<Result<bool>> CreateAppointmentAsync(ClaimsPrincipal principal, AppointmentCreateRequest request);
    Task<Result<bool>> DeleteAppointmentAsync(ClaimsPrincipal principal, string appointmentId);
    Task<PagedResult<AppointmentUserResponse>> GetAllAppointmentsOfOwnAsync(ClaimsPrincipal principal, AppointmentUserParameters param);
    Task<Result<AppointmentUserDetailResponse>> GetAppointmentOfOwnByIdAsync(ClaimsPrincipal principal, string appointmentId);

    /* Mentor get all appointments from student to them, view in detail, process accept or deny appointment, update info of appointment (real start, real finish time, mentor description about appointment) */
    Task<PagedResult<AppointmentMentorResponse>> GetAllAppointmentsOfMentorAsync(ClaimsPrincipal principal, AppointmentMentorParameters param);
    Task<Result<AppointmentMentorDetailResponse>> GetAppointmentOfMentorByIdAsync(ClaimsPrincipal principal, string appointmentId);
    Task<Result<bool>> ProcessAppointmentAsync(ClaimsPrincipal principal, string appointmentId, AppointmentProcessRequest request);
    Task<Result<bool>> MentorUpdateAppointmentAsync(ClaimsPrincipal principal, string appointmentId, AppointmentUpdateRequest request);

    /* Admin view all appointments of mentor, and can view appointment in detail */
    Task<PagedResult<AppointmentAdminResponse>> GetAllAppointmentsOfUserForAdminAsync(string userId, AppointmentAdminParameters param);
    Task<Result<AppointmentAdminDetailResponse>> GetAppointmentByIdForAdminAsync(string appointmentId);
}
