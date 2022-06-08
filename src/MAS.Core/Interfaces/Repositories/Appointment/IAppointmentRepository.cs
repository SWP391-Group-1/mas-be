using MAS.Core.Dtos.Incoming.Appointment;
using MAS.Core.Dtos.Outcoming.Appointment;
using MAS.Core.Dtos.Outcoming.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.Appointment
{
    public interface IAppointmentRepository
    {
        Task<Result<bool>> CreateAppointmentAsync(ClaimsPrincipal principal, string mentorId, AppointmentCreateRequest request);
        Task<Result<bool>> DeleteAppointmentAsync(string appointmentId);
        Task<PagedResult<AppointmentUserResponse>> GetAllAppointmentsOfOwnAsync(ClaimsPrincipal principal);
        Task<Result<AppointmentUserDetailResponse>> GetAppointmentOfOwnByIdAsync(ClaimsPrincipal principal, string appointmentId);
    }
}