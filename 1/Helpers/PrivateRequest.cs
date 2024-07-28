using _1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace _1.Helpers
{
    public class PrivateRequestViewModel
    {
        public string LastName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Patronymic { get; set; }

        public string? Phone { get; set; }

        public string Email { get; set; } = null!;

        public string Note { get; set; } = null!;

        public DateOnly Birthday { get; set; }

        public string PassportSeries { get; set; } = null!;

        public string PassportNumber { get; set; } = null!;

        public string? Organization { get; set; }

        public int UserId { get; set; }

        public byte[]? AvatarBytes { get; set; }

        public byte[]? PassportBytes { get; set; }

        public DateOnly DateFrom { get; set; }

        public DateOnly DateTo { get; set; }

        public int DepartmentId { get; set; }

        public int EmployeeId { get; set; }

        public int VisitPurposeId { get; set; }

        public Guest CopyToGuest()
        {
            var guest = new Guest
            {
                LastName = this.LastName,
                Name = this.Name,
                Patronymic = this.Patronymic,
                Phone = this.Phone,
                Email = this.Email,
                Organization = this.Organization,
                Note = this.Note,
                Birthday = this.Birthday,
                PassportSeries = this.PassportSeries,
                PassportNumber = this.PassportNumber,
                Avatar = this.AvatarBytes,
                Passport = this.PassportBytes,
                UserId = this.UserId
            };
            return guest;
        }

        public PrivateMeeting CopyToPrivateMeeting()
        {
            PrivateMeeting pm = new PrivateMeeting
            {
                DateFrom = this.DateFrom,
                DateTo = this.DateTo,
                DepartmentId = this.DepartmentId,
                EmployeeId = this.EmployeeId,
                VisitPurposeId = this.VisitPurposeId,
                StatusId = 1
            };
            return pm;
        }
        public GroupMeeting ConvertToGroupMeeting(int indexGroup)
        {
            GroupMeeting gm = new GroupMeeting
            {
                DateFrom = this.DateFrom,
                DateTo = this.DateTo,
                DeprtmentId = this.DepartmentId,
                EmployeeId = this.EmployeeId,
                VisitPurposeId = this.VisitPurposeId,
                StatusId = 1,
                GroupId = indexGroup
            };
            return gm;
        }
    }
}
