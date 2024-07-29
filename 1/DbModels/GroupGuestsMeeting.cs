using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class GroupGuestsMeeting
{
    public int Id { get; set; }

    public int GuestId { get; set; }

    public int GroupMeetingId { get; set; }

    public virtual GroupMeeting GroupMeeting { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;
}
