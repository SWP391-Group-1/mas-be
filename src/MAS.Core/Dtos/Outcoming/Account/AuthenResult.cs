using System;
using System.Collections.Generic;

namespace MAS.Core.Dtos.Outcoming.Account;

public class AuthenResult
{
    public string Message { get; set; }

    public bool Success => Errors is null;

    public List<string> Errors { get; set; }

    public DateTime ExpireDate { get; set; }
}
