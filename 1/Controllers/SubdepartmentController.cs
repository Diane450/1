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
        public async Task<string> Authorization([FromBody] string code)
        {
            if (int.TryParse(code, out int result))
            {
                var employee = await _dbContext.Employees.Where(e => e.Code == result).FirstOrDefaultAsync();
                if (employee != null && employee.Subdepartment == subdepartmentId)
                    return employee.FullName;
            }
            return null;
        }

        /// <summary>
        /// Возвращает список личных заявок
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AllRequestInfo>> GetPrivateRequests()
        {
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
            return privateRequests;
        }

        ///// <summary>
        ///// Возвращает список групповых заявок
        ///// </summary>
        ///// <returns></returns>
        [HttpGet]
        public async Task<List<AllRequestInfo>> GetGroupRequests()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<AllRequestInfo> groupRequests = await _dbContext.GroupMeetingsGuests
            .Include(group => group.Guest)
            .Include(group => group.GroupMeeting)
            .ThenInclude(meeting => meeting.Deprtment)
            .Include(group => group.GroupMeeting)
            .ThenInclude(meeting => meeting.Status)
            .Include(group => group.GroupMeeting)
            .ThenInclude(meeting => meeting.VisitPurpose)
            .Select(group => new AllRequestInfo
            {
                Guest = new GuestData
                {
                    Id = group.GuestId,
                    LastName = group.Guest.LastName,
                    Name = group.Guest.Name,
                    Patronymic = group.Guest.Patronymic,
                    Phone = group.Guest.Phone,
                    Email = group.Guest.Email,
                    Organization = group.Guest.Organization,
                    Note = group.Guest.Note,
                    Birthday = group.Guest.Birthday.ToString(),
                    PassportSeries = group.Guest.PassportSeries,
                    PassportNumber = group.Guest.PassportNumber,
                    AvatarBytes = group.Guest.Avatar,
                    PassportBytes = group.Guest.Passport
                },
                Meeting = new MeetingData
                {
                    Id = group.GroupMeeting.GroupMeetingId,
                    DateTo = group.GroupMeeting.DateTo,
                    DateFrom = group.GroupMeeting.DateFrom,
                    Status = new Status
                    {
                        Id = group.GroupMeeting.Status.IdStatus,
                        Name = group.GroupMeeting.Status.StatusName
                    },
                    VisitPurpose = group.GroupMeeting.VisitPurpose.Name,
                    Department = new _Department
                    {
                        Id = group.GroupMeeting.Deprtment.Id,
                        Name = group.GroupMeeting.Deprtment.DepartmentName
                    },
                    FullNameEmployee = group.GroupMeeting.Employee.FullName,
                    MeetingType = new _MeetingType
                    {
                        Id = 2,
                        Name = "Групповое посещение"
                    }
                }
            })
            .ToListAsync();

            stopwatch.Stop();
            return groupRequests;
        }

        /// <summary>
        /// Возвращает список отделов предприятия
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<_Department>> GetDepartmentsAsync()
        {
            return await _dbContext.Departments.AsNoTracking()
            .Select(d => new _Department
            {
                Id = d.Id,
                Name = d.DepartmentName
            })
            .ToListAsync();
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
        public async Task<DateOnly> GetPivateRequestVisitDate([FromBody] int id)
        {
            return await _dbContext.AcceptedPrivateRequests.Where(g => g.PrivateRequestId == id).Select(r => r.DateVisit).FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<DateOnly> GetGroupRequestVisitDate([FromBody] int id)
        {
            return await _dbContext.AcceptedGroupRequests.Where(g => g.GroupRequestId == id).Select(r => r.DateVisit).FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<TimeOnly> GetPrivateRequestVisitTime([FromBody] int id)
        {
            return await _dbContext.AcceptedPrivateRequests.Where(g => g.PrivateRequestId == id).Select(r => r.Time).FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<TimeOnly> GetGroupRequestVisitTime([FromBody] int id)
        {
            return await _dbContext.AcceptedGroupRequests.Where(g => g.GroupRequestId == id).Select(r => r.Time).FirstOrDefaultAsync();
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

        [HttpPost]
        public async Task DenyGroupRequestAsync([FromBody] _GroupDeniedRequest request)
        {
            GroupMeeting meeting = await _dbContext.GroupMeetings.Where(r => r.GroupMeetingId == request.GroupRequestId).FirstAsync();
            meeting.StatusId = 3;
            await _dbContext.GroupDeniedRequests.AddAsync(new GroupDeniedRequest
            {
                GroupRequestId = request.GroupRequestId,
                DeniedReasonId = request.DeniedReasonId
            });
            await _dbContext.SaveChangesAsync();
        }

        [HttpPost]
        public async Task AcceptPrivateRequestAsync([FromBody] _AcceptedPrivateRequest request)
        {
            PrivateMeeting meeting = await _dbContext.PrivateMeetings.Where(r => r.Id == request.PrivateRequestId).FirstAsync();
            meeting.StatusId = 2;
            await _dbContext.AcceptedPrivateRequests.AddAsync(new AcceptedPrivateRequest
            {
                PrivateRequestId = request.PrivateRequestId,
                Time = request.Time,
                DateVisit = request.DateVisit,
            });
            await _dbContext.SaveChangesAsync();
        }

        [HttpPost]
        public async Task AcceptGroupRequestAsync([FromBody] _AcceptedGroupRequest request)
        {
            GroupMeeting meeting = await _dbContext.GroupMeetings.Where(r => r.GroupMeetingId == request.GroupRequestId).FirstAsync();
            meeting.StatusId = 2;
            await _dbContext.AcceptedGroupRequests.AddAsync(new AcceptedGroupRequest
            {
                GroupRequestId = request.GroupRequestId,
                Time = request.Time,
                DateVisit = request.DateVisit,
            });
            await _dbContext.SaveChangesAsync();
        }

        
        [HttpPost]
        public async Task<int> GetAcceptedAmountRequest([FromBody] DateOnly[] range)
        {
            var privateRequestCount = await _dbContext.AcceptedPrivateRequests
                .Include(r => r.PrivateRequest)
                .Where(r => r.PrivateRequest.DateCreation > range[0] && r.PrivateRequest.DateCreation < range[1]).CountAsync();
            var groupRequestCount = await _dbContext.AcceptedGroupRequests
                .Include(r => r.GroupRequest)
                .Where(r => r.GroupRequest.DateCreation > range[0] && r.GroupRequest.DateCreation < range[1]).CountAsync();

            return privateRequestCount + groupRequestCount;
        }

        [HttpPost]
        public async Task<int> GetDeniedAmountRequest([FromBody] DateOnly[] range)
        {
            var privateRequestCount = await _dbContext.PrivateDeniedRequests
                .Include(r => r.PrivateRequest)
                .Where(r => r.PrivateRequest.DateCreation > range[0] && r.PrivateRequest.DateCreation < range[1]).CountAsync();
            var groupRequestCount = await _dbContext.GroupDeniedRequests
                .Include(r => r.GroupRequest)
                .Where(r => r.GroupRequest.DateCreation > range[0] && r.GroupRequest.DateCreation < range[1]).CountAsync();
            
            return privateRequestCount + groupRequestCount;
        }

        [HttpPost]
        public async Task<int> GetAllRequestsAmount([FromBody] DateOnly[] range)
        {
            var privateRequestCount = await _dbContext.PrivateMeetings
                .Where(r => r.DateCreation > range[0] && r.DateCreation < range[1]).CountAsync();

            var groupRequestCount = await _dbContext.GroupMeetings
                .Where(r => r.DateCreation > range[0] && r.DateCreation < range[1]).CountAsync();

            return privateRequestCount + groupRequestCount;
        }

        [HttpPost]
        public async Task<Dictionary<string, int>> GetPrivateRequestsReportDepartmentAsync([FromBody] DateOnly[]range)
        {
            var result = await _dbContext.PrivateMeetings
                .Where(r => r.DateCreation > range[0] && r.DateCreation < range[1])
                .GroupBy(e => e.Department.DepartmentName)
                .Select(g => new { DepartmentName = g.Key, Count = g.Count() })
                .ToListAsync();

            var allDepartments = await _dbContext.Departments.Select(d => d.DepartmentName).ToListAsync();

            var finalResult = allDepartments.ToDictionary(dep => dep, dep => result.FirstOrDefault(r => r.DepartmentName == dep)?.Count ?? 0);
            return finalResult;
        }

        [HttpPost]
        public async Task<Dictionary<string, int>> GetGroupRequestsReportDepartmentAsync([FromBody] DateOnly[] range)
        {
            var result = await _dbContext.GroupMeetings
                .Where(r => r.DateCreation > range[0] && r.DateCreation < range[1])
                .GroupBy(e => e.Deprtment.DepartmentName)
                .Select(g => new { DepartmentName = g.Key, Count = g.Count() })
                .ToListAsync();

            var allDepartments = await _dbContext.Departments.Select(d => d.DepartmentName).ToListAsync();

            var finalResult = allDepartments.ToDictionary(dep => dep, dep => result.FirstOrDefault(r => r.DepartmentName == dep)?.Count ?? 0);
            return finalResult;
        }
    }
}
