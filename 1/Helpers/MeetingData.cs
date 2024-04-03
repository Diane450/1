using _1.Helpers;
using System.ComponentModel;

namespace _1.Helpers
{
    public class MeetingData
    {
        public int Id { get; set; }

        public DateOnly DateTo { get; set; }

        public DateOnly DateFrom { get; set; }

        public Status Status { get; set; } = null!;

        public string VisitPurpose { get; set; } = null!;

        public _Department Department { get; set; } = null!;

        public string FullNameEmployee { get; set; } = null!;

        public _MeetingType MeetingType { get; set; } = null!;
    }
}
