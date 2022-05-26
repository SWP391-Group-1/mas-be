using MAS.Core.Dtos.Outcoming.Generic;

namespace MAS.Infrastructure.Helpers
{
    public static class ErrorHelper
    {
        public static Error PopulateError(int code, string type, string message)
        {
            return new Error {
                Code = code,
                Type = type,
                Message = message
            };
        }
    }
}
