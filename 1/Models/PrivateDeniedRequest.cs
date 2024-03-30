using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class PrivateDeniedRequest
{
    public int Id { get; set; }

    public int PrivateRequestId { get; set; }

    public int DeniedReasonId { get; set; }

    public virtual DeniedReason DeniedReason { get; set; } = null!;

    public virtual PrivateMeeting PrivateRequest { get; set; } = null!;
}
