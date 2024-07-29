using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class BlackListReason
{
    public int Id { get; set; }

    public string ShortName { get; set; } = null!;

    public string Descryption { get; set; } = null!;

    public virtual ICollection<GroupMeeting> GroupMeetings { get; set; } = new List<GroupMeeting>();

    public virtual ICollection<PrivateMeeting> PrivateMeetings { get; set; } = new List<PrivateMeeting>();
}
