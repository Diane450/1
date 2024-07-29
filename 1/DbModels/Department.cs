using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class Department
{
    public int Id { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<GroupMeeting> GroupMeetings { get; set; } = new List<GroupMeeting>();

    public virtual ICollection<PrivateMeeting> PrivateMeetings { get; set; } = new List<PrivateMeeting>();
}
