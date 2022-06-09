using System;

namespace MAS.Core.Dtos.Incoming.Generic
{
    public abstract class BaseUpdateRequest
    {
        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);
    }
}
