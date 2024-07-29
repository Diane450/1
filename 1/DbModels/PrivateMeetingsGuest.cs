using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class PrivateMeetingsGuest
{
    public int Id { get; set; }

    public int GuestId { get; set; }

    public int PrivateMeetingId { get; set; }

    public virtual Guest Guest { get; set; } = null!;

    public virtual PrivateMeeting PrivateMeeting { get; set; } = null!;
}
