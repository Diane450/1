using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class GroupMeetingsGuest
{
    public int Id { get; set; }

    public int GuestId { get; set; }

    public int GroupMeetingId { get; set; }

    public virtual GroupMeeting GroupMeeting { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;
}
