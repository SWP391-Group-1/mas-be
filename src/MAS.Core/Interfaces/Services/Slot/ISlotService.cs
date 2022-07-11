using MAS.Core.Dtos.Incoming.Slot;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Slot;
using MAS.Core.Parameters.Slot;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Services.Slot;

public interface ISlotService
{
    Task<Result<bool>> CreateAvailableSlotAsync(ClaimsPrincipal principal, SlotCreateRequest request);
    Task<Result<bool>> DeleteAvailableSlotAsync(ClaimsPrincipal principal, string slotId);
    Task<PagedResult<SlotResponse>> GetAllAvailableSlotsAsync(ClaimsPrincipal principal, SlotParameters param);
    Task<Result<SlotDetailResponse>> GetSlotByIdAsync(string slotId);
    Task<Result<bool>> CheckPassedSlotAsync();
}
