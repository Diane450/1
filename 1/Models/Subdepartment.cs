using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class Subdepartment
{
    public int Id { get; set; }

    public string SubdepartmentName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
