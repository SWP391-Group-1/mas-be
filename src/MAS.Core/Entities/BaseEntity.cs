using System;

namespace MAS.Core.Entities
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
