using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class AcceptedGroupRequest
{
    public int Id { get; set; }

    public int GroupRequestId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public virtual GroupMeeting GroupRequest { get; set; } = null!;
}
