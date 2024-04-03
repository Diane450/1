using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class CheckPrivateRequest
{
    public int Id { get; set; }

    public int PrivateRequestId { get; set; }

    public virtual PrivateMeeting PrivateRequest { get; set; } = null!;
}
