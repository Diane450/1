using _1.Helpers;
using System.ComponentModel;

namespace _1.Helpers
{
    public class MeetingData
    {
        public int Id { get; set; }

        public string DateTo { get; set; } = null!;

        public string DateFrom { get; set; } = null!;

        public string? DateVisit { get; set; }

        public string? Time { get; set; }

        public Status Status { get; set; } = null!;

        public string? DeniedReason { get; set; }

        public string VisitPurpose { get; set; } = null!;

        public _Department Department { get; set; } = null!;

        public string FullNameEmployee { get; set; } = null!;

        public _MeetingType MeetingType { get; set; } = null!;
    }
}
