namespace _1.Helpers
{
    public class _GroupDeniedRequest
    {
        public int Id { get; set; }

        public int GroupRequestId { get; set; }

        public int DeniedReasonId { get; set; }

        public DateOnly CreationDate { get; set; }
    }
}
