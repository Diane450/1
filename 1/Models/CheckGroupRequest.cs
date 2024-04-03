using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class CheckGroupRequest
{
    public int Id { get; set; }

    public int GroupRequestId { get; set; }

    public virtual GroupMeeting GroupRequest { get; set; } = null!;
}
