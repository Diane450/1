using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class GroupDeniedRequest
{
    public int Id { get; set; }

    public int GroupRequestId { get; set; }

    public int DeniedReasonId { get; set; }

    public virtual DeniedReason DeniedReason { get; set; } = null!;

    public virtual GroupMeeting GroupRequest { get; set; } = null!;
}
