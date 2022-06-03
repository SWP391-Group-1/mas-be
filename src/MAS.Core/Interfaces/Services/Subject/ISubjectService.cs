using MAS.Core.Dtos.Incoming.Subject;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Subject;
using MAS.Core.Parameters.Subject;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Services.Subject
{
    public interface ISubjectService
    {
        Task<Result<SubjectResponse>> CreateSubjectAsync(SubjectCreateRequest request);
        Task<PagedResult<SubjectResponse>> GetAllSubjectsAsync(SubjectParameters param);
        Task<Result<SubjectDetailResponse>> GetSubjectByIdAsync(string subjectId);
        Task<Result<bool>> DeleteSubjectAsync(string subjectId);
        Task<Result<bool>> UpdateSubjectAsync(string subjectId, SubjectUpdateRequest request);
    }
}