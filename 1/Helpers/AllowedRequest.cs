namespace _1.Helpers
{
    public class AllowedRequest
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public string StartTime { get; set; }

        public string? EnterTime { get; set; }

        public string? EndingTime { get; set; }

        public string? CompletionTime { get; set; }
        public string MeetingType { get; set; }
    }
}
