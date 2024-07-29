using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class AcceptedGroupRequest
{
    public int Id { get; set; }

    public int GroupRequestId { get; set; }

    public DateOnly DateVisit { get; set; }

    public TimeOnly Time { get; set; }

    public virtual GroupMeeting GroupRequest { get; set; } = null!;
}
