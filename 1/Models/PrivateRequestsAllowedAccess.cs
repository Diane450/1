using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class PrivateRequestsAllowedAccess
{
    public int Id { get; set; }

    public int PrivateRequestId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly? EnterTime { get; set; }

    public TimeOnly? EndingTime { get; set; }

    public TimeOnly? CompletionTime { get; set; }

    public virtual PrivateMeeting PrivateRequest { get; set; } = null!;
}
