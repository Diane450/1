using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class DeniedReason
{
    public int Id { get; set; }

    public string ShortName { get; set; } = null!;

    public string Descryption { get; set; } = null!;

    public virtual ICollection<GroupDeniedRequest> GroupDeniedRequests { get; set; } = new List<GroupDeniedRequest>();

    public virtual ICollection<PrivateDeniedRequest> PrivateDeniedRequests { get; set; } = new List<PrivateDeniedRequest>();
}
