using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class User
{
    public int IdUser { get; set; }

    public string Email { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public int UserRoleId { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();

    public virtual UserRole UserRole { get; set; } = null!;
}
