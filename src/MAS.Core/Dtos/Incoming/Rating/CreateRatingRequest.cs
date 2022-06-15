using MAS.Core.Dtos.Incoming.Generic;

namespace MAS.Core.Dtos.Incoming.Rating
{
    public class CreateRatingRequest : BaseCreateRequest
    {
        public int Vote { get; set; }
        public string Comment { get; set; }
    }
}
