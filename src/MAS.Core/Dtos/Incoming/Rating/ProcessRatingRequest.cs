using MAS.Core.Dtos.Incoming.Generic;

namespace MAS.Core.Dtos.Incoming.Rating
{
    public class ProcessRatingRequest : BaseUpdateRequest
    {
        public bool IsApprove { get; set; }
    }
}
