using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class Guest
{
    public int IdGuests { get; set; }

    public string LastName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public string Note { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public string PassportSeries { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public string? Organization { get; set; }

    public int? UserId { get; set; }

    public byte[]? Avatar { get; set; }

    public byte[]? Passport { get; set; }

    public virtual ICollection<BlackListGuest> BlackListGuests { get; set; } = new List<BlackListGuest>();

    public virtual ICollection<GroupMeetingsGuest> GroupMeetingsGuests { get; set; } = new List<GroupMeetingsGuest>();

    public virtual ICollection<PrivateMeetingsGuest> PrivateMeetingsGuests { get; set; } = new List<PrivateMeetingsGuest>();

    public virtual User? User { get; set; }
}
