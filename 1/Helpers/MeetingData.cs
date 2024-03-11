using System.ComponentModel;

namespace _1.Helpers
{
    public class MeetingData
    {
        public int Id { get; set; }

        public string DateTo { get; set; } = null!;

        public string DateFrom { get; set; } = null!;

        public string? DateVisit;

        private string? Time;

        private string Status = null!;

        public string VisitPurpose { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string FullNameEmployee { get; set; } = null!;
    }
}
