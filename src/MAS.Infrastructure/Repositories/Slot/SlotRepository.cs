using AutoMapper;
using MAS.Core.Dtos.Incoming.Slot;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Slot;
using MAS.Core.Interfaces.Repositories.Slot;
using MAS.Core.Parameters.Slot;
using MAS.Infrastructure.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.Slot
{
    public class SlotRepository : BaseRepository, ISlotRepository
    {
        public SlotRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<Result<bool>> CreateAvailableSlotAsync(ClaimsPrincipal principal, SlotCreateRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<bool>> DeleteAvailableSlotAsync(ClaimsPrincipal principal, string slotId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedResult<SlotResponse>> GetAllAvailableSlotsAsync(SlotParameters param)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<SlotDetailResponse>> GetSlotByIdAsync(string slotId)
        {
            throw new System.NotImplementedException();
        }
    }
}