using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class VisitPurpose
{
    public int IdVisitPurpose { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PrivateMeeting> PrivateMeetings { get; set; } = new List<PrivateMeeting>();
}
