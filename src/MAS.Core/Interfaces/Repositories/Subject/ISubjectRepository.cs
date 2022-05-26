using MAS.Core.Dtos.Incoming.Subject;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Subject;
using MAS.Core.Parameters.Subject;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.Subject
{
    public interface ISubjectRepository
    {
        Task<Result<SubjectResponse>> CreateSubjectAsync(SubjectCreateRequest request);
        Task<PagedResult<SubjectResponse>> GetAllSubjectsAsync(SubjectParameters param);
        Task<Result<SubjectResponse>> GetSubjectByIdAsync(string subjectId);
        Task<Result<bool>> DeleteSubjectAsync(string subjectId);
        Task<Result<bool>> UpdateSubjectAsync(string subjectId, SubjectUpdateRequest request);
    }
}
