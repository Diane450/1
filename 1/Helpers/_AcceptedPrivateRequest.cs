﻿namespace _1.Helpers
{
    public class _AcceptedPrivateRequest
    {
        public int Id { get; set; }

        public int PrivateRequestId { get; set; }

        public DateOnly DateVisit { get; set; }

        public TimeOnly Time { get; set; }

        public string ClientEmail { get; set; } = null!;

        public DateOnly CreationDate { get; set; }
    }
}
