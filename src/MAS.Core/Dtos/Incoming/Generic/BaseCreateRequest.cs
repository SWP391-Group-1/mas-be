using System;

namespace MAS.Core.Dtos.Incoming.Generic
{
    public class BaseCreateRequest
    {
        public string Id = Guid.NewGuid().ToString();
        public DateTime CreateDate = DateTime.UtcNow;
    }
}
