using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class DeniedRequestPrivateMeeting
{
    public int Id { get; set; }

    public int PrivateMeetingId { get; set; }

    public int DeniedReasonId { get; set; }

    public virtual BlackListReason DeniedReason { get; set; } = null!;

    public virtual PrivateMeeting PrivateMeeting { get; set; } = null!;
}
