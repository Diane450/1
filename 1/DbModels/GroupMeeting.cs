﻿using System;
using System.Collections.Generic;

namespace _1.DbModels;

public partial class GroupMeeting
{
    public int GroupMeetingId { get; set; }

    public DateOnly DateCreation { get; set; }

    public DateOnly DateFrom { get; set; }

    public DateOnly DateTo { get; set; }

    public int DeprtmentId { get; set; }

    public int EmployeeId { get; set; }

    public int GroupId { get; set; }

    public int StatusId { get; set; }

    public int VisitPurposeId { get; set; }

    public virtual ICollection<AcceptedGroupRequest> AcceptedGroupRequests { get; set; } = new List<AcceptedGroupRequest>();

    public virtual Department Deprtment { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

    public virtual ICollection<GroupDeniedRequest> GroupDeniedRequests { get; set; } = new List<GroupDeniedRequest>();

    public virtual ICollection<GroupMeetingsGuest> GroupMeetingsGuests { get; set; } = new List<GroupMeetingsGuest>();

    public virtual ICollection<GroupRequestsAllowedAccess> GroupRequestsAllowedAccesses { get; set; } = new List<GroupRequestsAllowedAccess>();

    public virtual MeetingStatus Status { get; set; } = null!;

    public virtual VisitPurpose VisitPurpose { get; set; } = null!;
}
