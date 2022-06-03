using MAS.Core.Dtos.Outcoming.Major;

namespace MAS.Core.Dtos.Outcoming.Subject
{
    public class SubjectDetailResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public MajorResponse Major { get; set; }
    }
}