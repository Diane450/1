using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class VisitPurpose
{
    public int IdVisitPurpose { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GroupMeeting> GroupMeetings { get; set; } = new List<GroupMeeting>();

    public virtual ICollection<PrivateMeeting> PrivateMeetings { get; set; } = new List<PrivateMeeting>();
}
