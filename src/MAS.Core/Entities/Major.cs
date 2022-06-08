using MAS.Core.Entities.Abtraction;
using System.Collections.Generic;

namespace MAS.Core.Entities
{
    public class Major : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<Subject> Subjects { get; set; }
    }
}
