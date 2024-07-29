using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class BlackListGuest
{
    public int Id { get; set; }

    public int GuestId { get; set; }

    public string Reason { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;
}
