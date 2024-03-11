using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class GroupRequestsAllowedAccess
{
    public int Id { get; set; }

    public int GroupRequestsId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly? EnterTime { get; set; }

    public TimeOnly? EndingTime { get; set; }

    public TimeOnly? CompletionTime { get; set; }

    public virtual GroupMeeting GroupRequests { get; set; } = null!;
}
