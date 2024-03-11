using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class AllRequest
{
    public int Id { get; set; }

    public int? PrivateRequestsId { get; set; }

    public int? GroupRequestsId { get; set; }

    public int TypeId { get; set; }

    public virtual GroupMeeting? GroupRequests { get; set; }

    public virtual PrivateMeeting? PrivateRequests { get; set; }

    public virtual MeetingType Type { get; set; } = null!;
}
