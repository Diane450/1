﻿using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class Employee
{
    public int IdEmployees { get; set; }

    public string FullName { get; set; } = null!;

    public int? Department { get; set; }

    public int? Subdepartment { get; set; }

    public int Code { get; set; }

    public virtual Department? DepartmentNavigation { get; set; }

    public virtual ICollection<GroupMeeting> GroupMeetings { get; set; } = new List<GroupMeeting>();

    public virtual ICollection<PrivateMeeting> PrivateMeetings { get; set; } = new List<PrivateMeeting>();

    public virtual Subdepartment? SubdepartmentNavigation { get; set; }
}
