﻿using MAS.Core.Dtos.Incoming.Major;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Major;
using MAS.Core.Parameters.Major;
using System.Threading.Tasks;

namespace MAS.Core.Interfaces.Repositories.Major;

public interface IMajorRepository
{
    Task<Result<MajorResponse>> CreateMajorAsync(MajorCreateRequest request);
    Task<PagedResult<MajorResponse>> GetAllMajorsAsync(MajorParameters param);
    Task<Result<MajorResponse>> GetMajorByIdAsync(string MajorId);
    Task<Result<bool>> DeleteMajorAsync(string MajorId);
    Task<Result<bool>> UpdateMajorAsync(string MajorId, MajorUpdateRequest request);
}
