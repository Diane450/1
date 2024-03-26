using _1.Helpers;
using _1.Interfaces;
using _1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubdepartmentController : ControllerBase
    {
        private Ispr2438IbragimovaDm1Context _dbContext;
        private readonly IEmailSender _emailSender;
        private static readonly int subdepartmentId = 1;

        public SubdepartmentController(Ispr2438IbragimovaDm1Context _dbContext, IEmailSender emailSender)
        {
            this._dbContext = _dbContext;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Авторизация сотрдуников общего отдела и охраны
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Authorization([FromBody] string code)
        {
            if (int.TryParse(code, out int result))
            {
                var employee = await _dbContext.Employees.Where(e => e.Code == result).FirstOrDefaultAsync();
                return employee != null && employee.Subdepartment == subdepartmentId;
            }
            return false;
        }

        /// <summary>
        /// Возвращает список личных заявок
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AllRequestInfo>> GetPrivateRequests()
        {
            List<AllRequestInfo> privateRequests = await (from privateMeetingGuests in _dbContext.PrivateMeetingsGuests
                                                          join guest in _dbContext.Guests on privateMeetingGuests.GuestId equals guest.IdGuests
                                                          join privateMeeting in _dbContext.PrivateMeetings on privateMeetingGuests.PrivateMeetingId equals privateMeeting.Id
                                                          join deniedReasons in _dbContext.DeniedReasons on privateMeeting.DeniedReasonId equals deniedReasons.Id
                                                          into deniedReasonsGroup
                                                          from deniedReason in deniedReasonsGroup.DefaultIfEmpty()
                                                          join department in _dbContext.Departments on privateMeeting.DepartmentId equals department.Id
                                                          join employee in _dbContext.Employees on privateMeeting.EmployeeId equals employee.IdEmployees
                                                          join status in _dbContext.MeetingStatuses on privateMeeting.StatusId equals status.IdStatus
                                                          join purpose in _dbContext.VisitPurposes on privateMeeting.VisitPurposeId equals purpose.IdVisitPurpose
                                                          select new AllRequestInfo()
                                                          {
                                                              Guest = new GuestData
                                                              {
                                                                  Id = privateMeetingGuests.GuestId,
                                                                  LastName = guest.LastName,
                                                                  Name = guest.Name,
                                                                  Patronymic = guest.Patronymic,
                                                                  Phone = guest.Phone,
                                                                  Email = guest.Email,
                                                                  Organization = guest.Organization,
                                                                  Note = guest.Note,
                                                                  Birthday = guest.Birthday.ToString(),
                                                                  PassportSeries = guest.PassportSeries,
                                                                  PassportNumber = guest.PassportNumber,
                                                                  AvatarBytes = guest.Avatar,
                                                                  PassportBytes = guest.Passport
                                                              },
                                                              Meeting = new MeetingData()
                                                              {
                                                                  Id = privateMeeting.Id,
                                                                  DateTo = privateMeeting.DateTo.ToString(),
                                                                  DateFrom = privateMeeting.DateFrom.ToString(),
                                                                  DateVisit = privateMeeting.DateVisit.ToString(),
                                                                  Time = privateMeeting.Time.ToString(),
                                                                  Status = new Status
                                                                  {
                                                                      Id = status.IdStatus,
                                                                      Name = status.StatusName
                                                                  },
                                                                  DeniedReason = deniedReason.ShortName,
                                                                  VisitPurpose = purpose.Name,
                                                                  Department = new _Department
                                                                  {
                                                                      Id = department.Id,
                                                                      Name = department.DepartmentName
                                                                  },
                                                                  FullNameEmployee = employee.FullName,
                                                                  MeetingType = new _MeetingType
                                                                  {
                                                                      Id = 1,
                                                                      Name = "Личное посещение"
                                                                  }
                                                              }
                                                          }).ToListAsync();
            return privateRequests;
        }

        ///// <summary>
        ///// Возвращает список групповых заявок
        ///// </summary>
        ///// <returns></returns>
        [HttpGet]
        public async Task<List<AllRequestInfo>> GetGroupRequests()
        {
            List<AllRequestInfo> groupRequests = await (from groupMeetingGuests in _dbContext.GroupMeetingsGuests
                                                        join guest in _dbContext.Guests on groupMeetingGuests.GuestId equals guest.IdGuests
                                                        join groupMeeting in _dbContext.GroupMeetings on groupMeetingGuests.GroupMeetingId equals groupMeeting.GroupMeetingId
                                                        join deniedReasons in _dbContext.DeniedReasons on groupMeeting.DeniedReasonId equals deniedReasons.Id
                                                        into deniedReasonsGroup
                                                        from deniedReason in deniedReasonsGroup.DefaultIfEmpty()
                                                        join department in _dbContext.Departments on groupMeeting.DeprtmentId equals department.Id
                                                        join employee in _dbContext.Employees on groupMeeting.EmployeeId equals employee.IdEmployees
                                                        join status in _dbContext.MeetingStatuses on groupMeeting.StatusId equals status.IdStatus
                                                        join purpose in _dbContext.VisitPurposes on groupMeeting.VisitPurposeId equals purpose.IdVisitPurpose
                                                        select new AllRequestInfo()
                                                        {
                                                            Guest = new GuestData
                                                            {
                                                                Id = groupMeetingGuests.GuestId,
                                                                LastName = guest.LastName,
                                                                Name = guest.Name,
                                                                Patronymic = guest.Patronymic,
                                                                Phone = guest.Phone,
                                                                Email = guest.Email,
                                                                Organization = guest.Organization,
                                                                Note = guest.Note,
                                                                Birthday = guest.Birthday.ToString(),
                                                                PassportSeries = guest.PassportSeries,
                                                                PassportNumber = guest.PassportNumber,
                                                                AvatarBytes = guest.Avatar,
                                                                PassportBytes = guest.Passport
                                                            },
                                                            Meeting = new MeetingData()
                                                            {
                                                                Id = groupMeeting.GroupMeetingId,
                                                                DateTo = groupMeeting.DateTo.ToString(),
                                                                DateFrom = groupMeeting.DateFrom.ToString(),
                                                                DateVisit = groupMeeting.DateVisit.ToString(),
                                                                Time = groupMeeting.Time.ToString(),
                                                                Status = new Status
                                                                {
                                                                    Id = status.IdStatus,
                                                                    Name = status.StatusName
                                                                },
                                                                DeniedReason = deniedReason.ShortName,
                                                                VisitPurpose = purpose.Name,
                                                                Department = new _Department
                                                                {
                                                                    Id = department.Id,
                                                                    Name = department.DepartmentName
                                                                },
                                                                FullNameEmployee = employee.FullName,
                                                                MeetingType = new _MeetingType
                                                                {
                                                                    Id = 2,
                                                                    Name = "Групповое посещение"
                                                                }
                                                            }
                                                        }).ToListAsync();
            return groupRequests;
        }

        /// <summary>
        /// Возвращает список отделов предприятия
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<_Department>> GetDepartmentsAsync()
        {
            return (await (from d in _dbContext.Departments
                                        select new _Department
                                        {
                                            Id = d.Id,
                                            Name = d.DepartmentName
                                        }).ToListAsync());
        }

        /// <summary>
        /// Возвращает список статусов заявок
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Status>> GetStatusesAsync()
        {
            return (await (from s in _dbContext.MeetingStatuses
                                     select new Status
                                     {
                                         Id = s.IdStatus,
                                         Name = s.StatusName
                                     }).ToListAsync());
        }

        /// <summary>
        /// Возвращает список типов заявок
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<_MeetingType> GetMeetingTypes()
        {
            return new List<_MeetingType>
            {
                new _MeetingType
                {
                    Id = 0,
                    Name = "Все"
                },
                new _MeetingType
                {
                    Id = 1,
                    Name = "Личное посещение"
                },
                new _MeetingType
                {
                    Id = 2,
                    Name = "Групповое посещение"
                }
            };
        }
    }
}
