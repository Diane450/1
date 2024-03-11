using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class GroupGuest
{
    public int IdGuestsGroup { get; set; }

    public string LastName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? Phone { get; set; }

    public string? Organization { get; set; }

    public string? Note { get; set; }

    public DateTime Birthday { get; set; }

    public int PassportSeries { get; set; }

    public int PassportNumber { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<GuestsGroupMeeting> GuestsGroupMeetings { get; set; } = new List<GuestsGroupMeeting>();

    public virtual User User { get; set; } = null!;
}
