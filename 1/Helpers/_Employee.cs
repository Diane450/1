using _1.Models;

namespace _1.Helpers
{
    public class _Employee
    {
        public int IdEmployees { get; set; }

        public string FullName { get; set; } = null!;

        public _Department? Department { get; set; }

        public _Subdepartment? Subdepartment { get; set; }

        public int Code { get; set; }

        public byte[]? Photo { get; set; }

        public int PassportNumber { get; set; }

        public int PassportSeries { get; set; }

        public _EmployeeUserType? EmployeeUserType { get; set; }
    }
}
