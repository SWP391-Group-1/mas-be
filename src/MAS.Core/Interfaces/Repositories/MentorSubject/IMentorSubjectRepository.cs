using MAS.Core.Dtos.Incoming.MentorSubject;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.MentorSubject;
using MAS.Core.Parameters.MentorSubject;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.MentorSubject
{
    public interface IMentorSubjectRepository
    {
        Task<Result<bool>> RegisterSubjectAsync(ClaimsPrincipal principal, MentorSubjectRegisterRequest request);
        Task<PagedResult<MentorSubjectResponse>> GetAllsSubjectOfMentorAsync(string mentorId, MentorSubjectParameters param);
        Task<Result<bool>> UpdateSubjectOfMentorAsync(ClaimsPrincipal principal, string subjectOfMentorId, MentorSubjectUpdateRequest request);
        Task<Result<bool>> DeleteSubjectOfMentorAsync(ClaimsPrincipal principal, string subjectOfMentorId);
    }
}
