using System;

namespace MAS.Core.Entities.Abtraction;

public abstract class BaseEntity
{
    public string Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool IsActive { get; set; }
}
