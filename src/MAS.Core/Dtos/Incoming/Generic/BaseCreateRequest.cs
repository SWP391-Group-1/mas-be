using System;

namespace MAS.Core.Dtos.Incoming.Generic
{
    public abstract class BaseCreateRequest
    {
        public string Id = Guid.NewGuid().ToString();
        public DateTime CreateDate = DateTime.UtcNow.AddHours(7);
    }
}
