using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class MeetingStatus
{
    public int IdStatus { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<GroupMeeting> GroupMeetings { get; set; } = new List<GroupMeeting>();

    public virtual ICollection<PrivateMeeting> PrivateMeetings { get; set; } = new List<PrivateMeeting>();
}
