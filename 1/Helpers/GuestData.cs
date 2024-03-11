using System.ComponentModel;

namespace _1.Helpers
{
    public class GuestData
    {
        public int Id { get; set; }

        public string LastName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Patronymic { get; set; }

        public string? Phone { get; set; }

        public string Email { get; set; } = null!;

        public string? Organization { get; set; }

        public string Note { get; set; } = null!;

        public string Birthday { get; set; } = null!;

        public string PassportSeries { get; set; } = null!;

        public string PassportNumber { get; set; } = null!;

        public byte[]? AvatarBytes { get; set; }

        public byte[]? PassportBytes { get; set; }
    }
}
