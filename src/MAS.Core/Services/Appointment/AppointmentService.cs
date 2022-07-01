using MAS.Core.Dtos.Incoming.Appointment;
using MAS.Core.Dtos.Outcoming.Appointment;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Interfaces.Repositories.Appointment;
using MAS.Core.Interfaces.Services.Appointment;
using MAS.Core.Parameters.Appointment;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Services.Appointment;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        ILogger<AppointmentService> logger)
    {
        _appointmentRepository = appointmentRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> CreateAppointmentAsync(
        ClaimsPrincipal principal,
        AppointmentCreateRequest request)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _appointmentRepository.CreateAppointmentAsync(principal, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call CreateAppointmentAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> DeleteAppointmentAsync(
        ClaimsPrincipal principal,
        string appointmentId)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (String.IsNullOrEmpty(appointmentId) || String.IsNullOrWhiteSpace(appointmentId)) {
                throw new ArgumentNullException(nameof(appointmentId));
            }
            return await _appointmentRepository.DeleteAppointmentAsync(principal, appointmentId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call DeleteAppointmentAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<AppointmentMentorResponse>> GetAllReceiveAppointmentsAsync(
        ClaimsPrincipal principal,
        AppointmentMentorParameters param)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            return await _appointmentRepository.GetAllReceiveAppointmentsAsync(principal, param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllReceiveAppointmentsAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<AppointmentAdminResponse>> GetAllAppointmentsOfUserForAdminAsync(
        string userId,
        AppointmentAdminParameters param)
    {
        try {
            if (String.IsNullOrEmpty(userId) || String.IsNullOrWhiteSpace(userId)) {
                throw new ArgumentNullException(nameof(userId));
            }
            return await _appointmentRepository.GetAllAppointmentsOfUserForAdminAsync(userId, param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllAppointmentsOfUserForAdminAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<AppointmentUserResponse>> GetAllSendAppointmentsAsync(
        ClaimsPrincipal principal,
        AppointmentUserParameters param)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            return await _appointmentRepository.GetAllSendAppointmentsAsync(principal, param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllSendAppointmentsAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<AppointmentAdminDetailResponse>> GetAppointmentByIdForAdminAsync(string appointmentId)
    {
        try {
            if (String.IsNullOrEmpty(appointmentId) || String.IsNullOrWhiteSpace(appointmentId)) {
                throw new ArgumentNullException(nameof(appointmentId));
            }
            return await _appointmentRepository.GetAppointmentByIdForAdminAsync(appointmentId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAppointmentByIdForAdminAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<AppointmentMentorDetailResponse>> GetAppointmentReceiveByIdAsync(
        ClaimsPrincipal principal,
        string appointmentId)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (String.IsNullOrEmpty(appointmentId) || String.IsNullOrWhiteSpace(appointmentId)) {
                throw new ArgumentNullException(nameof(appointmentId));
            }
            return await _appointmentRepository.GetAppointmentReceiveByIdAsync(principal, appointmentId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAppointmentReceiveByIdAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<AppointmentUserDetailResponse>> GetAppointmentSendedByIdAsync(
        ClaimsPrincipal principal,
        string appointmentId)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (String.IsNullOrEmpty(appointmentId) || String.IsNullOrWhiteSpace(appointmentId)) {
                throw new ArgumentNullException(nameof(appointmentId));
            }
            return await _appointmentRepository.GetAppointmentSendedByIdAsync(principal, appointmentId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAppointmentSendedByIdAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> MentorUpdateAppointmentAsync(
        ClaimsPrincipal principal,
        string appointmentId,
        AppointmentUpdateRequest request)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (String.IsNullOrEmpty(appointmentId) || String.IsNullOrWhiteSpace(appointmentId)) {
                throw new ArgumentNullException(nameof(appointmentId));
            }
            return await _appointmentRepository.MentorUpdateAppointmentAsync(principal, appointmentId, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call MentorUpdateAppointmentAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> ProcessAppointmentAsync(
        ClaimsPrincipal principal,
        string appointmentId,
        AppointmentProcessRequest request)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            if (String.IsNullOrEmpty(appointmentId) || String.IsNullOrWhiteSpace(appointmentId)) {
                throw new ArgumentNullException(nameof(appointmentId));
            }
            return await _appointmentRepository.ProcessAppointmentAsync(principal, appointmentId, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call ProcessAppointmentAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> CheckPassedAppointmentAsync()
    {
        try {
            return await _appointmentRepository.CheckPassedAppointmentAsync();
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call CheckPassedAppointmentAsync in service class, Error Message: {ex}.");
            throw;
        }
    }
}
