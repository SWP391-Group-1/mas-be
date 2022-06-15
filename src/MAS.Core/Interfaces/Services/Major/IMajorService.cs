using MAS.Core.Dtos.Incoming.Major;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Major;
using MAS.Core.Parameters.Major;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Services.Major;

public interface IMajorService
{
    Task<Result<MajorResponse>> CreateMajorAsync(MajorCreateRequest request);
    Task<PagedResult<MajorResponse>> GetAllMajorsAsync(MajorParameters param);
    Task<Result<MajorResponse>> GetMajorByIdAsync(string majorId);
    Task<Result<bool>> DeleteMajorAsync(string majorId);
    Task<Result<bool>> UpdateMajorAsync(string majorId, MajorUpdateRequest request);
}
