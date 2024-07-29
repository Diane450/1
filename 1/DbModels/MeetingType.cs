using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class MeetingType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;
}
