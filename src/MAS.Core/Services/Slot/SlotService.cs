using MAS.Core.Dtos.Incoming.Slot;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Slot;
using MAS.Core.Interfaces.Repositories.Slot;
using MAS.Core.Interfaces.Services.Slot;
using MAS.Core.Parameters.Slot;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Services.Slot;

public class SlotService : ISlotService
{
    private readonly ISlotRepository _slotRepository;
    private readonly ILogger<SlotService> _logger;

    public SlotService(ISlotRepository slotRepository, ILogger<SlotService> logger)
    {
        _logger = logger;
        _slotRepository = slotRepository;

    }

    public async Task<Result<bool>> CheckPassedSlotAsync()
    {
        try {
            return await _slotRepository.CheckPassedSlotAsync();
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call CheckPassedSlotAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> CreateAvailableSlotAsync(ClaimsPrincipal principal, SlotCreateRequest request)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (request is null) {
                throw new ArgumentNullException(nameof(request));
            }
            return await _slotRepository.CreateAvailableSlotAsync(principal, request);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call CreateAvailableSlotAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<bool>> DeleteAvailableSlotAsync(ClaimsPrincipal principal, string slotId)
    {
        try {
            if (principal is null) {
                throw new ArgumentNullException(nameof(principal));
            }
            if (String.IsNullOrEmpty(slotId) || String.IsNullOrWhiteSpace(slotId)) {
                throw new ArgumentNullException(nameof(slotId));
            }
            return await _slotRepository.DeleteAvailableSlotAsync(principal, slotId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call DeleteAvailableSlotAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<PagedResult<SlotResponse>> GetAllAvailableSlotsAsync(SlotParameters param)
    {
        try {
            return await _slotRepository.GetAllAvailableSlotsAsync(param);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetAllAvailableSlotsAsync in service class, Error Message: {ex}.");
            throw;
        }
    }

    public async Task<Result<SlotDetailResponse>> GetSlotByIdAsync(string slotId)
    {
        try {
            if (String.IsNullOrEmpty(slotId) || String.IsNullOrWhiteSpace(slotId)) {
                throw new ArgumentNullException(nameof(slotId));
            }
            return await _slotRepository.GetSlotByIdAsync(slotId);
        }
        catch (Exception ex) {
            _logger.LogError($"Error while trying to call GetSlotByIdAsync in service class, Error Message: {ex}.");
            throw;
        }
    }
}
