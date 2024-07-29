using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class AcceptedPrivateRequest
{
    public int Id { get; set; }

    public int PrivateRequestId { get; set; }

    public DateOnly DateVisit { get; set; }

    public TimeOnly Time { get; set; }

    public virtual PrivateMeeting PrivateRequest { get; set; } = null!;
}
