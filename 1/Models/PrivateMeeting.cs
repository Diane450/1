using System;
using System.Collections.Generic;

namespace _1.Models;

public partial class PrivateMeeting
{
    public int Id { get; set; }

    public DateOnly DateFrom { get; set; }

    public DateOnly DateTo { get; set; }

    public DateOnly? DateVisit { get; set; }

    public TimeOnly? Time { get; set; }

    public int DepartmentId { get; set; }

    public int EmployeeId { get; set; }

    public int VisitPurposeId { get; set; }

    public int StatusId { get; set; }

    public int? DeniedReasonId { get; set; }

    public virtual DeniedReason? DeniedReason { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<PrivateMeetingsGuest> PrivateMeetingsGuests { get; set; } = new List<PrivateMeetingsGuest>();

    public virtual ICollection<PrivateRequestsAllowedAccess> PrivateRequestsAllowedAccesses { get; set; } = new List<PrivateRequestsAllowedAccess>();

    public virtual MeetingStatus Status { get; set; } = null!;

    public virtual VisitPurpose VisitPurpose { get; set; } = null!;
}
