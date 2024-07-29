using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class Group
{
    public int IdGroups { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GroupMeeting> GroupMeetings { get; set; } = new List<GroupMeeting>();
}
