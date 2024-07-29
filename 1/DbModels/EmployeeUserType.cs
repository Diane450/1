using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class EmployeeUserType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
