namespace _1.Helpers
{
    public class _PrivateDeniedRequest
    {
        public int Id { get; set; }

        public int PrivateRequestId { get; set; }

        public int DeniedReasonId { get; set; }

        public string ClientEmail { get; set; } = null!;

        public DateOnly CreationDate { get; set; }
    }
}
