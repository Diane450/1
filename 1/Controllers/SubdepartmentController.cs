using _1.Helpers;
using _1.Interfaces;
using _1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            Stopwatch sw = Stopwatch.StartNew();
            //List<AllRequestInfo> privateRequests = await (from privateMeetingGuests in _dbContext.PrivateMeetingsGuests
            //                                              join guest in _dbContext.Guests on privateMeetingGuests.GuestId equals guest.IdGuests
            //                                              join privateMeeting in _dbContext.PrivateMeetings on privateMeetingGuests.PrivateMeetingId equals privateMeeting.Id
            //                                              join department in _dbContext.Departments on privateMeeting.DepartmentId equals department.Id
            //                                              join employee in _dbContext.Employees on privateMeeting.EmployeeId equals employee.IdEmployees
            //                                              join status in _dbContext.MeetingStatuses on privateMeeting.StatusId equals status.IdStatus
            //                                              join purpose in _dbContext.VisitPurposes on privateMeeting.VisitPurposeId equals purpose.IdVisitPurpose
            //                                              select new AllRequestInfo()
            //                                              {
            //                                                  Guest = new GuestData
            //                                                  {
            //                                                      Id = privateMeetingGuests.GuestId,
            //                                                      LastName = guest.LastName,
            //                                                      Name = guest.Name,
            //                                                      Patronymic = guest.Patronymic,
            //                                                      Phone = guest.Phone,
            //                                                      Email = guest.Email,
            //                                                      Organization = guest.Organization,
            //                                                      Note = guest.Note,
            //                                                      Birthday = guest.Birthday.ToString(),
            //                                                      PassportSeries = guest.PassportSeries,
            //                                                      PassportNumber = guest.PassportNumber,
            //                                                      AvatarBytes = guest.Avatar,
            //                                                      PassportBytes = guest.Passport
            //                                                  },
            //                                                  Meeting = new MeetingData()
            //                                                  {
            //                                                      Id = privateMeeting.Id,
            //                                                      DateTo = privateMeeting.DateTo,
            //                                                      DateFrom = privateMeeting.DateFrom,
            //                                                      Status = new Status
            //                                                      {
            //                                                          Id = status.IdStatus,
            //                                                          Name = status.StatusName
            //                                                      },
            //                                                      VisitPurpose = purpose.Name,
            //                                                      Department = new _Department
            //                                                      {
            //                                                          Id = department.Id,
            //                                                          Name = department.DepartmentName
            //                                                      },
            //                                                      FullNameEmployee = employee.FullName,
            //                                                      MeetingType = new _MeetingType
            //                                                      {
            //                                                          Id = 1,
            //                                                          Name = "Личное посещение"
            //                                                      }
            //                                                  }
            //                                              }).ToListAsync();
            List<AllRequestInfo> privateRequests = await _dbContext.PrivateMeetingsGuests
    .Include(pmg => pmg.Guest)
    .Include(pmg => pmg.PrivateMeeting)
        .ThenInclude(pm => pm.Status)
    .Include(pmg => pmg.PrivateMeeting)
        .ThenInclude(pm => pm.VisitPurpose)
    .Include(pmg => pmg.PrivateMeeting)
        .ThenInclude(pm => pm.Department)
    .Include(pmg => pmg.PrivateMeeting)
        .ThenInclude(pm => pm.Employee)
    .Select(pmg => new AllRequestInfo()
    {
        Guest = new GuestData
        {
            Id = pmg.GuestId,
            LastName = pmg.Guest.LastName,
            Name = pmg.Guest.Name,
            Patronymic = pmg.Guest.Patronymic,
            Phone = pmg.Guest.Phone,
            Email = pmg.Guest.Email,
            Organization = pmg.Guest.Organization,
            Note = pmg.Guest.Note,
            Birthday = pmg.Guest.Birthday.ToString(),
            PassportSeries = pmg.Guest.PassportSeries,
            PassportNumber = pmg.Guest.PassportNumber,
            AvatarBytes = pmg.Guest.Avatar,
            PassportBytes = pmg.Guest.Passport
        },
        Meeting = new MeetingData()
        {
            Id = pmg.PrivateMeeting.Id,
            DateTo = pmg.PrivateMeeting.DateTo,
            DateFrom = pmg.PrivateMeeting.DateFrom,
            Status = new Status
            {
                Id = pmg.PrivateMeeting.Status.IdStatus,
                Name = pmg.PrivateMeeting.Status.StatusName
            },
            VisitPurpose = pmg.PrivateMeeting.VisitPurpose.Name,
            Department = new _Department
            {
                Id = pmg.PrivateMeeting.Department.Id,
                Name = pmg.PrivateMeeting.Department.DepartmentName
            },
            FullNameEmployee = pmg.PrivateMeeting.Employee.FullName,
            MeetingType = new _MeetingType
            {
                Id = 1,
                Name = "Личное посещение"
            }
        }
    })
    .ToListAsync();


            sw.Stop();
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
                                                                DateTo = groupMeeting.DateTo,
                                                                DateFrom = groupMeeting.DateFrom,
                                                                Status = new Status
                                                                {
                                                                    Id = status.IdStatus,
                                                                    Name = status.StatusName
                                                                },
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


        /// <summary>
        /// Возвращает список причин отказа заявки
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<_DeniedReason>> GetDeniedReasons()
        {
            return await (from d in _dbContext.DeniedReasons
                          select new _DeniedReason
                          {
                              Id = d.Id,
                              ShortName = d.ShortName
                          }).ToListAsync();
        }

        /// <summary>
        /// Возваращает причину отказа личной заявки
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<_DeniedReason> GetPrivateRequestDeniedReason([FromBody] int requestId)
        {
            return await (from privateDeniedRequest in _dbContext.PrivateDeniedRequests
                          join deniedReason in _dbContext.DeniedReasons on privateDeniedRequest.DeniedReasonId equals deniedReason.Id
                          where privateDeniedRequest.PrivateRequestId == requestId
                          select new _DeniedReason
                          {
                              Id = deniedReason.Id,
                              ShortName = deniedReason.ShortName
                          }).FirstAsync();
        }

        /// <summary>
        /// Возвращает причину отказа групповой заявки
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<_DeniedReason> GetGroupRequestDeniedReason([FromBody] int requestId)
        {
            return await (from groupDeniedRequest in _dbContext.GroupDeniedRequests
                          join deniedReason in _dbContext.DeniedReasons on groupDeniedRequest.DeniedReasonId equals deniedReason.Id
                          where groupDeniedRequest.GroupRequestId == requestId
                          select new _DeniedReason
                          {
                              Id = deniedReason.Id,
                              ShortName = deniedReason.ShortName
                          }).FirstAsync();
        }

        [HttpPost]
        public async Task<bool> IsGuestBlackListed([FromBody] int id)
        {
            return await _dbContext.BlackListGuests.Where(g => g.GuestId == id).FirstOrDefaultAsync() != null;
        }

        [HttpPost]
        public async Task DenyPrivateRequestAsync([FromBody] _PrivateDeniedRequest request)
        {
            PrivateMeeting meeting = await _dbContext.PrivateMeetings.Where(r => r.Id == request.PrivateRequestId).FirstAsync();
            meeting.StatusId = 3;
            await _dbContext.PrivateDeniedRequests.AddAsync(new PrivateDeniedRequest
            {
                PrivateRequestId = request.PrivateRequestId,
                DeniedReasonId = request.DeniedReasonId
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}
