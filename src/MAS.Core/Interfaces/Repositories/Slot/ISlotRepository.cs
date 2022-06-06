﻿using MAS.Core.Dtos.Incoming.Slot;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Slot;
using MAS.Core.Parameters.Slot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.Slot
{
    public interface ISlotRepository
    {
        Task<Result<bool>> CreateAvailableSlotAsync(ClaimsPrincipal principal, SlotCreateRequest request);
        Task<Result<bool>> DeleteAvailableSlotAsync(ClaimsPrincipal principal, string slotId);
        Task<PagedResult<SlotResponse>> GetAllAvailableSlotsAsync(SlotParameters param);
        Task<Result<SlotDetailResponse>> GetSlotByIdAsync(string slotId);
    }
}
