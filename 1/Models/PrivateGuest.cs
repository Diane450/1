using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class PrivateGuest
{
    public int IdGuests { get; set; }

    public string LastName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? Phone { get; set; }

    public string? Organization { get; set; }

    public string Note { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public int PasssportSeries { get; set; }

    public int PassportNumber { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<PrivateMeetingsGuest> PrivateMeetingsGuests { get; set; } = new List<PrivateMeetingsGuest>();

    public virtual User User { get; set; } = null!;
}
