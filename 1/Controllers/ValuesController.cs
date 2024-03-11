using _1.classes;
using _1.Helpers;
using _1.Interfaces;
using _1.Models;
using BCrypt.Net;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Group = _1.Models.Group;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private Ispr2438IbragimovaDm1Context _dbContext;
        private readonly IEmailSender _emailSender;
        public ValuesController(Ispr2438IbragimovaDm1Context _dbContext, IEmailSender emailSender)
        {
            this._dbContext = _dbContext;
            _emailSender = emailSender;
        }
        //регистрация
        [HttpPost]
        public async Task<int> Register([FromBody] AuthBody value)
        {
            User user = new User
            {
                Email = value.Email,
                Salt = BCrypt.Net.BCrypt.GenerateSalt()
            };
            user.Password = BCrypt.Net.BCrypt.HashPassword(value.Password, user.Salt!);
            user.Login = value.Login;
            CurrentUser.login = user.Login;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.IdUser;
        }

        [HttpPost]
        public async Task<bool> CheckLogin([FromBody] string login)
        {
            return await _dbContext.Users.Where(u => u.Login == login).AnyAsync();
        }

        //авторизация
        [HttpPost]
        public async Task<int> Auth([FromBody] AuthBody value)
        {
            var user = await _dbContext.Users.Where((user) => user.Login == value.Login).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.Password == BCrypt.Net.BCrypt.HashPassword(value.Password, user.Salt!))
                {
                    return user.IdUser;
                }
                return 0;
            }
            return 0;
        }
        //инф-ция о прошлых заявках пользователя
        [HttpPost]
        public async Task<IEnumerable<MeetingInfo>> GetPreviousRequests([FromBody] int id)
        {
            var meetingInfo = await (from privateMeetingGuests in _dbContext.PrivateMeetingsGuests
                                     join guests in _dbContext.Guests on privateMeetingGuests.GuestId equals guests.IdGuests
                                     join privateMeeting in _dbContext.PrivateMeetings on privateMeetingGuests.PrivateMeetingId equals privateMeeting.Id
                                     join reason in _dbContext.DeniedReasons on privateMeeting.DeniedReasonId equals reason.Id
                                     into deniedReasonsGroup
                                     from deniedReason in deniedReasonsGroup.DefaultIfEmpty()
                                     join department in _dbContext.Departments on privateMeeting.DepartmentId equals department.Id
                                     join status in _dbContext.MeetingStatuses on privateMeeting.StatusId equals status.IdStatus
                                     join user in _dbContext.Users on guests.UserId equals user.IdUser
                                     where user.IdUser == id
                                     select new MeetingInfo
                                     {
                                         Department = department.DepartmentName,
                                         Time = privateMeeting.Time,
                                         DateVisit = privateMeeting.DateVisit,
                                         TypeMeeting = "Личное посещение",
                                         Status = status.StatusName,
                                         Reason = deniedReason.Descryption
                                     }).Union
                               (from guestsGroupMeetings in _dbContext.GroupMeetingsGuests
                                join guests in _dbContext.Guests on guestsGroupMeetings.GuestId equals guests.IdGuests
                                join groupMeeting in _dbContext.GroupMeetings on guestsGroupMeetings.GroupMeetingId equals groupMeeting.GroupMeetingId
                                join reason in _dbContext.DeniedReasons on groupMeeting.DeniedReasonId equals reason.Id
                                into deniedReasonsGroup
                                from deniedReason in deniedReasonsGroup.DefaultIfEmpty()
                                join department in _dbContext.Departments on groupMeeting.DeprtmentId equals department.Id
                                join status in _dbContext.MeetingStatuses on groupMeeting.StatusId equals status.IdStatus
                                join user in _dbContext.Users on guests.UserId equals user.IdUser
                                where user.IdUser == id
                                select new MeetingInfo
                                {
                                    Department = department.DepartmentName,
                                    Time = groupMeeting.Time,
                                    DateVisit = groupMeeting.DateVisit,
                                    TypeMeeting = "Групповое посещение",
                                    Status = status.StatusName,
                                    Reason = deniedReason.Descryption
                                }).ToListAsync();
            return meetingInfo;
        }

        //создание private meeting пользователем
        [HttpPost]
        public async Task CreatePrivateRequest([FromBody] PrivateRequestViewModel value)
        {
            Guest guest = value.CopyToGuest();
            await _dbContext.Guests.AddAsync(guest);
            await _dbContext.SaveChangesAsync();

            PrivateMeeting privateMeeting = value.CopyToPrivateMeeting();
            await _dbContext.PrivateMeetings.AddAsync(privateMeeting);
            await _dbContext.SaveChangesAsync();

            var privateMeetingGuests = new PrivateMeetingsGuest();
            privateMeetingGuests.GuestId = guest.IdGuests;
            privateMeetingGuests.PrivateMeetingId = privateMeeting.Id;
            await _dbContext.PrivateMeetingsGuests.AddAsync(privateMeetingGuests);
            await _dbContext.SaveChangesAsync();
        }

        //создание новой группы
        [HttpPost]
        public int CreateNewGroup(PrivateRequestViewModel value)
        {
            string department = (from d in _dbContext.Departments where d.Id == value.DepartmentId select d.DepartmentName).First();
            string employee = (from e in _dbContext.Employees where e.IdEmployees == value.EmployeeId select e.FullName).First();
            int code = (int)(from e in _dbContext.Employees where e.FullName == employee select e.Code).First();
            employee = employee.Split(' ')[0];
            DateTime date = DateTime.Now;
            string groupName = $"{date.Day}/{date.Month}/{date.Year}_{department}_{employee}_{code}_";
            int count = (from groups in _dbContext.Groups select groups.Name).Where(d => d.StartsWith(groupName)).ToList().Count();
            if (count > 0)
            {
                groupName += $"ГР{count + 1}";
            }
            else
            {
                groupName += $"ГР1";

            }
            Group group = new Group()
            {
                Name = groupName
            };
            _dbContext.Groups.Add(group);
            _dbContext.SaveChanges();
            return group.IdGroups;
        }

        //создание групповой заявк
        [HttpPost]
        public async Task CreateGroupRequest([FromBody] List<PrivateRequestViewModel>  value)
        {
            int indexGroup = CreateNewGroup(value[0]);
            GroupMeeting groupmeeting = value[0].ConvertToGroupMeeting(indexGroup);
            await _dbContext.GroupMeetings.AddAsync(groupmeeting);
            await _dbContext.SaveChangesAsync();

            for (int i = 0; i < value.Count; i++)
            {
                Guest guest = value[0].CopyToGuest();
                await _dbContext.Guests.AddAsync(guest);
                await _dbContext.SaveChangesAsync();

                var groupMeetingGuests = new GroupMeetingsGuest();
                groupMeetingGuests.GuestId = guest.IdGuests;
                groupMeetingGuests.GroupMeetingId = groupmeeting.GroupMeetingId;
                await _dbContext.GroupMeetingsGuests.AddAsync(groupMeetingGuests);
                await _dbContext.SaveChangesAsync();
            }
        }


        //[HttpGet]
        //public async Task<List<AllRequestInfo>> GetAllRequests()
        //{
        //    var requests = await (from privateMeetingGuests in _dbContext.PrivateMeetingsGuests
        //                          join guest in _dbContext.Guests on privateMeetingGuests.GuestId equals guest.IdGuests
        //                          join privateMeeting in _dbContext.PrivateMeetings on privateMeetingGuests.PrivateMeetingId equals privateMeeting.Id
        //                          join deniedReasons in _dbContext.DeniedReasons on privateMeeting.DeniedReasonId equals deniedReasons.Id
        //                          into deniedReasonsGroup
        //                          from deniedReason in deniedReasonsGroup.DefaultIfEmpty()
        //                          join department in _dbContext.Departments on privateMeeting.DepartmentId equals department.Id
        //                          join employee in _dbContext.Employees on privateMeeting.EmployeeId equals employee.IdEmployees
        //                          join status in _dbContext.MeetingStatuses on privateMeeting.StatusId equals status.IdStatus
        //                          join purpose in _dbContext.VisitPurposes on privateMeeting.VisitPurposeId equals purpose.IdVisitPurpose
        //                          select new AllRequestInfo
        //                          {
        //                              Id = guest.IdGuests,
        //                              LastName = guest.LastName,
        //                              Name = guest.Name,
        //                              Patronymic = guest.Patronymic,
        //                              Phone = guest.Phone,
        //                              Email = guest.Email,
        //                              Organization = guest.Organization,
        //                              Note = guest.Note,
        //                              Birthday = guest.Birthday.ToString("yyyy-MM-dd"),
        //                              PassportSeries = guest.PassportSeries,
        //                              PassportNumber = guest.PassportNumber,
        //                              DateFrom = privateMeeting.DateFrom.ToString("yyyy-MM-dd"),
        //                              DateTo = privateMeeting.DateTo.ToString("yyyy-MM-dd"),
        //                              Department = department.DepartmentName,
        //                              FullNameEmployee = employee.FullName,
        //                              AvatarBytes = guest.Avatar,
        //                              VisitPurpose = purpose.Name,
        //                              MeetingType = "Личное посещение",
        //                              Status = status.StatusName,
        //                              MeetingId = privateMeeting.Id,
        //                              DeniedReason = deniedReason.ShortName,
        //                              PassportBytes = guest.Passport
        //                          }).ToListAsync();
        //    var r = await (from groupMeetingGuests in _dbContext.GroupMeetingsGuests
        //                   join guest in _dbContext.Guests on groupMeetingGuests.GuestId equals guest.IdGuests
        //                   join groupMeeting in _dbContext.GroupMeetings on groupMeetingGuests.GroupMeetingId equals groupMeeting.GroupMeetingId
        //                   join deniedReasons in _dbContext.DeniedReasons on groupMeeting.DeniedReasonId equals deniedReasons.Id
        //                   into deniedReasonsGroup
        //                   from deniedReason in deniedReasonsGroup.DefaultIfEmpty()
        //                   join department in _dbContext.Departments on groupMeeting.DeprtmentId equals department.Id
        //                   join employee in _dbContext.Employees on groupMeeting.EmployeeId equals employee.IdEmployees
        //                   join status in _dbContext.MeetingStatuses on groupMeeting.StatusId equals status.IdStatus
        //                   join purpose in _dbContext.VisitPurposes on groupMeeting.VisitPurposeId equals purpose.IdVisitPurpose
        //                   select new AllRequestInfo
        //                   {
        //                       Id = guest.IdGuests,
        //                       LastName = guest.LastName,
        //                       Name = guest.Name,
        //                       Patronymic = guest.Patronymic,
        //                       Phone = guest.Phone,
        //                       Email = guest.Email,
        //                       Organization = guest.Organization,
        //                       Note = guest.Note,
        //                       Birthday = guest.Birthday.ToString("yyyy-MM-dd"),
        //                       PassportSeries = guest.PassportSeries,
        //                       PassportNumber = guest.PassportNumber,
        //                       DateFrom = groupMeeting.DateFrom.ToString("yyyy-MM-dd"),
        //                       DateTo = groupMeeting.DateTo.ToString("yyyy-MM-dd"),
        //                       Department = department.DepartmentName,
        //                       FullNameEmployee = employee.FullName,
        //                       AvatarBytes = guest.Avatar,
        //                       VisitPurpose = purpose.Name,
        //                       MeetingType = "Групповое посещение",
        //                       Status = status.StatusName,
        //                       MeetingId = groupMeeting.GroupMeetingId,
        //                       DeniedReason = deniedReason.ShortName,
        //                       PassportBytes = guest.Passport
        //                   }).ToListAsync();
        //    requests.AddRange(r);
        //    return requests;
        //}   

        [HttpGet]
        public async Task<List<Department>> GetDepartments()
        {
            return await _dbContext.Departments.ToListAsync();
        }

        [HttpGet]
        public async Task<List<EmployeeSelectList>> GetEmployees()
        {
            return await _dbContext.Employees.Where(e => e.Department != null).Select(e => new EmployeeSelectList
            {
                IdEmployees = e.IdEmployees,
                FullName = e.FullName,
                Department = e.Department
            }).ToListAsync();
        }

        [HttpGet]
        public async Task<List<VisitPurpose>> GetPurposes()
        {
            return await _dbContext.VisitPurposes.ToListAsync();
        }

        //[HttpGet]
        //public async Task<List<Mee>> GetMeetingType()
        //{
        //    var meetingTypes = new List<string>
        //    {
        //        "Все"
        //    };
        //    meetingTypes.AddRange(await _dbContext.MeetingTypes.Select(t => t.Type).ToListAsync());
        //    return meetingTypes;
        //}

        [HttpPost]
        public async Task<bool> IsGuestBlackListed([FromBody] string passportNumber)
        {
            var guest = await (from blackList in _dbContext.BlackListGuests
                               join g in _dbContext.Guests on blackList.GuestId equals g.IdGuests
                               where g.PassportNumber == passportNumber
                               select blackList).FirstOrDefaultAsync();
            return guest != null;
        }
        [HttpGet]
        public async Task<List<MeetingStatus>> GetStatuses()
        {
            var statuses = await _dbContext.MeetingStatuses.ToListAsync();
            return statuses;
        }
        [HttpGet]
        public async Task<List<string>> GetDeniedReasons()
        {
            var reasons = await _dbContext.DeniedReasons.Select(r => r.ShortName).ToListAsync();
            return reasons;
        }
        //[HttpPost]
        //public async Task DenyRequest([FromBody] AllRequestInfo request)
        //{
        //    int deniedReasonId = await (from deniedReason in _dbContext.DeniedReasons
        //                                where deniedReason.ShortName == request.DeniedReason
        //                                select deniedReason.Id).FirstOrDefaultAsync();
        //    var receiver = new List<string>();
        //    MailMessage message = new MailMessage();
        //    message.Subject = "Заяка отклонена";
        //    if (request.MeetingType == "Личное посещение")
        //    {
        //        PrivateMeeting privateMeeting = await (from pm in _dbContext.PrivateMeetings
        //                                               where pm.Id == request.MeetingId
        //                                               select pm).FirstOrDefaultAsync();

        //        privateMeeting.StatusId = 3;
        //        privateMeeting.DeniedReasonId = deniedReasonId;
        //        await _dbContext.SaveChangesAsync();
        //        message.Body = await (from DeniedReasons in _dbContext.DeniedReasons
        //                              where DeniedReasons.ShortName == request.DeniedReason
        //                              select DeniedReasons.Descryption).FirstOrDefaultAsync();

        //        receiver.Add(request.Email);
        //    }
        //    else
        //    {
        //        GroupMeeting groupMeeting = await (from gm in _dbContext.GroupMeetings
        //                                           where gm.GroupMeetingId == request.MeetingId
        //                                           select gm).FirstOrDefaultAsync();

        //        groupMeeting.StatusId = 2;
        //        groupMeeting.DeniedReasonId = deniedReasonId;
        //        await _dbContext.SaveChangesAsync();

        //        message.Body = await (from DeniedReasons in _dbContext.DeniedReasons
        //                              where DeniedReasons.ShortName == request.DeniedReason
        //                              select DeniedReasons.Descryption).FirstOrDefaultAsync();

        //        receiver = await (from groupMeetingGuests in _dbContext.GroupMeetingsGuests
        //                          join guests in _dbContext.Guests on groupMeetingGuests.GuestId equals guests.IdGuests
        //                          where groupMeetingGuests.GroupMeetingId == request.MeetingId
        //                          select guests.Email).ToListAsync();
        //    }
        //    _emailSender.SendEmailAsync(receiver, message);
        //}

        ////[HttpPost]
        ////public async Task AddGuestToBlackList([FromBody] int Id)
        ////{
        ////    int count = await (from privateMeetingGuest in _dbContext.PrivateMeetingsGuests
        ////                       join privateMeeting in _dbContext.PrivateMeetings on privateMeetingGuest.PrivateMeetingId equals privateMeeting.Id
        ////                       where privateMeetingGuest.Id == Id
        ////                       select privateMeetingGuest).CountAsync();
        ////    count += await (from groupMeetingGuest in _dbContext.GroupMeetingsGuests
        ////                    join groupMeeting in _dbContext.GroupMeetings on groupMeetingGuest.GroupMeetingId equals groupMeeting.GroupMeetingId
        ////                    where groupMeetingGuest.Id == Id
        ////                    select groupMeetingGuest).CountAsync();
        ////    if (count > 1)
        ////    {
        ////        await _dbContext.BlackListGuests.AddAsync(new BlackListGuest
        ////        {
        ////            GuestId = Id
        ////        });
        ////    }
        ////}
        //[HttpPost]
        //public async Task AddGuestToBlackList([FromBody] BlackListGuest guest)
        //{
        //    var guestBlackList = new _1.Models.BlackListGuest
        //    {
        //        GuestId = guest.GuestId,
        //        Reason = guest.Reason
        //    };
        //    await _dbContext.BlackListGuests.AddAsync(guestBlackList);
        //    _dbContext.SaveChanges();
        //}
        //[HttpPost]
        //public async Task AcceptRequest([FromBody] AllRequestInfo request)
        //{
        //    var date = DateTime.Parse(request.DateVisit).ToString("dd.MM.yyyy");
        //    var receiver = new List<string>();
        //    if (request.MeetingType == "Личное посещение")
        //    {
        //        var privateRequest = await (from privateMeeting in _dbContext.PrivateMeetings
        //                                    where privateMeeting.Id == request.MeetingId
        //                                    select privateMeeting).FirstOrDefaultAsync();
        //        privateRequest.Time = TimeOnly.Parse(request.Time);
        //        privateRequest.DateVisit = DateOnly.Parse(request.DateVisit);
        //        privateRequest.StatusId = 2;
        //        await _dbContext.SaveChangesAsync();
        //        receiver.Add(request.Email);
        //    }
        //    else
        //    {
        //        var groupRequest = await (from groupMeeting in _dbContext.GroupMeetings
        //                                  where groupMeeting.GroupMeetingId == request.MeetingId
        //                                  select groupMeeting).FirstOrDefaultAsync();
        //        groupRequest.Time = TimeOnly.Parse(request.Time);
        //        groupRequest.DateVisit = DateOnly.Parse(request.DateVisit);
        //        groupRequest.StatusId = 2;
        //        await _dbContext.SaveChangesAsync();

        //        receiver = await (from groupMeetingGuests in _dbContext.GroupMeetingsGuests
        //                          join guests in _dbContext.Guests on groupMeetingGuests.GuestId equals guests.IdGuests
        //                          where groupMeetingGuests.GroupMeetingId == request.MeetingId
        //                          select guests.Email).ToListAsync();
        //    }
        //    MailMessage message = new MailMessage();
        //    message.Body = $"Заявка на посещение объекта КИИ одобрена, дата посещения {date}, время посещения: {request.Time}";
        //    message.Subject = "Заявка одобрена";

        //    _emailSender.SendEmailAsync(receiver, message);
        //}

        //[HttpGet]
        //public async Task<List<AllRequestInfo>> GetAcceptedRequests()
        //{
        //    var requests = await (from privateMeetingGuests in _dbContext.PrivateMeetingsGuests
        //                          join guest in _dbContext.Guests on privateMeetingGuests.GuestId equals guest.IdGuests
        //                          join privateMeeting in _dbContext.PrivateMeetings on privateMeetingGuests.PrivateMeetingId equals privateMeeting.Id
        //                          join department in _dbContext.Departments on privateMeeting.DepartmentId equals department.Id
        //                          join employee in _dbContext.Employees on privateMeeting.EmployeeId equals employee.IdEmployees
        //                          join status in _dbContext.MeetingStatuses on privateMeeting.StatusId equals status.IdStatus
        //                          join purpose in _dbContext.VisitPurposes on privateMeeting.VisitPurposeId equals purpose.IdVisitPurpose
        //                          where status.IdStatus == 2
        //                          select new AllRequestInfo
        //                          {
        //                              Id = guest.IdGuests,
        //                              LastName = guest.LastName,
        //                              Name = guest.Name,
        //                              Patronymic = guest.Patronymic,
        //                              Phone = guest.Phone,
        //                              Email = guest.Email,
        //                              Organization = guest.Organization,
        //                              Note = guest.Note,
        //                              Birthday = guest.Birthday.ToString("yyyy-MM-dd"),
        //                              PassportSeries = guest.PassportSeries,
        //                              PassportNumber = guest.PassportNumber,
        //                              DateFrom = privateMeeting.DateFrom.ToString("yyyy-MM-dd"),
        //                              DateTo = privateMeeting.DateTo.ToString("yyyy-MM-dd"),
        //                              DateVisit = privateMeeting.DateVisit.ToString(),
        //                              Department = department.DepartmentName,
        //                              FullNameEmployee = employee.FullName,
        //                              AvatarBytes = guest.Avatar,
        //                              VisitPurpose = purpose.Name,
        //                              MeetingType = "Личное посещение",
        //                              Status = status.StatusName,
        //                              MeetingId = privateMeeting.Id
        //                              //PassportBytes = guest.Passport
        //                          }).ToListAsync();
        //    var r = await (from groupMeetingGuests in _dbContext.GroupMeetingsGuests
        //                   join guest in _dbContext.Guests on groupMeetingGuests.GuestId equals guest.IdGuests
        //                   join groupMeeting in _dbContext.GroupMeetings on groupMeetingGuests.GroupMeetingId equals groupMeeting.GroupMeetingId
        //                   join department in _dbContext.Departments on groupMeeting.DeprtmentId equals department.Id
        //                   join employee in _dbContext.Employees on groupMeeting.EmployeeId equals employee.IdEmployees
        //                   join status in _dbContext.MeetingStatuses on groupMeeting.StatusId equals status.IdStatus
        //                   join purpose in _dbContext.VisitPurposes on groupMeeting.VisitPurposeId equals purpose.IdVisitPurpose
        //                   where status.IdStatus == 2
        //                   select new AllRequestInfo
        //                   {
        //                       Id = guest.IdGuests,
        //                       LastName = guest.LastName,
        //                       Name = guest.Name,
        //                       Patronymic = guest.Patronymic,
        //                       Phone = guest.Phone,
        //                       Email = guest.Email,
        //                       Organization = guest.Organization,
        //                       Note = guest.Note,
        //                       Birthday = guest.Birthday.ToString("yyyy-MM-dd"),
        //                       PassportSeries = guest.PassportSeries,
        //                       PassportNumber = guest.PassportNumber,
        //                       DateFrom = groupMeeting.DateFrom.ToString("yyyy-MM-dd"),
        //                       DateTo = groupMeeting.DateTo.ToString("yyyy-MM-dd"),
        //                       DateVisit = groupMeeting.DateVisit.ToString(),
        //                       Department = department.DepartmentName,
        //                       FullNameEmployee = employee.FullName,
        //                       AvatarBytes = guest.Avatar,
        //                       VisitPurpose = purpose.Name,
        //                       MeetingType = "Групповое посещение",
        //                       Status = status.StatusName,
        //                       MeetingId = groupMeeting.GroupMeetingId,
        //                       //PassportBytes = guest.Passport
        //                   }).ToListAsync();
        //    requests.AddRange(r);
        //    return requests;
        //}

        //[HttpPost]
        //public async Task AllowAccess([FromBody] AllRequestInfo request)
        //{
        //    var time = DateTime.Now.TimeOfDay;
        //    if (request.MeetingType == "Личное посещение")
        //    {
        //        var allowedAccess = new PrivateRequestsAllowedAccess
        //        {
        //            PrivateRequestId = request.MeetingId,
        //            StartTime = new TimeOnly(Convert.ToInt64(time))
        //        };
        //        await _dbContext.PrivateRequestsAllowedAccesses.AddAsync(allowedAccess);
        //    }
        //    else
        //    {
        //        var allowedAccess = new GroupRequestsAllowedAccess
        //        {
        //            GroupRequestsId = request.MeetingId,
        //            StartTime = new TimeOnly(Convert.ToInt64(time))
        //        };
        //        await _dbContext.GroupRequestsAllowedAccesses.AddAsync(allowedAccess);
        //    }
        //    await _dbContext.SaveChangesAsync();
        //}
        ////[HttpPost]
        ////public async Task<AllowedRequest> IsPrivateAllowedAccess([FromBody] AllRequestInfo request)
        ////{
        ////    var req = await _dbContext.PrivateRequestsAllowedAccesses.Where(r => r.PrivateRequestId == request.MeetingId).FirstOrDefaultAsync();
        ////    var types = await _dbContext.MeetingTypes.ToListAsync();
        ////    if (req != null)
        ////    {
        ////        AllowedRequest allowedRequest = new AllowedRequest
        ////        {
        ////            Id = req.Id,
        ////            StartTime = req.StartTime.ToString(),
        ////            EnterTime = req.EnterTime.ToString(),
        ////            EndingTime = req.EndingTime.ToString(),
        ////            CompletionTime = req.CompletionTime.ToString(),
        ////            MeetingType = types[0].Type
        ////        };
        ////        return allowedRequest;
        ////    }
        ////    return null;
        ////}

        ////[HttpPost]
        ////public async Task<AllowedRequest> IsGroupAllowedAccess([FromBody] AllRequestInfo request)
        ////{

        ////    var req = await _dbContext.GroupRequestsAllowedAccesses.Where(r => r.GroupRequestsId == request.MeetingId).FirstOrDefaultAsync();
        ////    var types = await _dbContext.MeetingTypes.ToListAsync();
        ////    if (req != null)
        ////    {
        ////        AllowedRequest allowedRequest = new AllowedRequest
        ////        {
        ////            Id = req.Id,
        ////            StartTime = req.StartTime.ToString(),
        ////            EnterTime = req.EnterTime.ToString(),
        ////            EndingTime = req.EndingTime.ToString(),
        ////            CompletionTime = req.CompletionTime.ToString(),
        ////            MeetingType = types[1].Type
        ////        };
        ////        return allowedRequest;
        ////    }
        ////    return null;
        ////}

        ////[HttpPost]
        ////public async Task SetCompletionTimeAsync([FromBody] AllowedRequest request)
        ////{
        ////    var types = await _dbContext.MeetingTypes.ToListAsync();
        ////    if (request.MeetingType == types[0].Type)
        ////    {
        ////        var req = await _dbContext.PrivateRequestsAllowedAccesses.Where(r => r.Id == request.Id).FirstOrDefaultAsync();
        ////        req.CompletionTime = TimeOnly.Parse(request.CompletionTime);
        ////        await _dbContext.SaveChangesAsync();
        ////    }
        ////    else
        ////    {
        ////        var req = await _dbContext.GroupRequestsAllowedAccesses.Where(r => r.Id == request.Id).FirstOrDefaultAsync();
        ////        req.CompletionTime = TimeOnly.Parse(request.CompletionTime);
        ////        await _dbContext.SaveChangesAsync();
        ////    }
        ////}

        //[HttpPost]
        //public async Task<Employee> DepartmentAuth([FromBody] int code)
        //{
        //    return await _dbContext.Employees.Where(e => e.Code == code).FirstOrDefaultAsync();
        //}
        //[HttpPost]
        //public async Task<bool> test([FromBody] Guest g)
        //{
        //    return true;
        //}
        //[HttpPost]
        //public async Task<List<AllRequestInfo>> GetDepartmentRequest([FromBody] int departmentId)
        //{
        //    var requests = await (from privateMeetingGuests in _dbContext.PrivateMeetingsGuests
        //                          join guest in _dbContext.Guests on privateMeetingGuests.GuestId equals guest.IdGuests
        //                          join privateMeeting in _dbContext.PrivateMeetings on privateMeetingGuests.PrivateMeetingId equals privateMeeting.Id
        //                          join department in _dbContext.Departments on privateMeeting.DepartmentId equals department.Id
        //                          join employee in _dbContext.Employees on privateMeeting.EmployeeId equals employee.IdEmployees
        //                          join status in _dbContext.MeetingStatuses on privateMeeting.StatusId equals status.IdStatus
        //                          join purpose in _dbContext.VisitPurposes on privateMeeting.VisitPurposeId equals purpose.IdVisitPurpose
        //                          where department.Id == departmentId
        //                          where status.IdStatus == 2
        //                          select new AllRequestInfo
        //                          {
        //                              Id = guest.IdGuests,
        //                              LastName = guest.LastName,
        //                              Name = guest.Name,
        //                              Patronymic = guest.Patronymic,
        //                              Phone = guest.Phone,
        //                              Email = guest.Email,
        //                              Organization = guest.Organization,
        //                              Note = guest.Note,
        //                              Birthday = guest.Birthday.ToString("yyyy-MM-dd"),
        //                              PassportSeries = guest.PassportSeries,
        //                              PassportNumber = guest.PassportNumber,
        //                              DateFrom = privateMeeting.DateFrom.ToString("yyyy-MM-dd"),
        //                              DateTo = privateMeeting.DateTo.ToString("yyyy-MM-dd"),
        //                              DateVisit = privateMeeting.DateVisit.ToString(),
        //                              Department = department.DepartmentName,
        //                              FullNameEmployee = employee.FullName,
        //                              AvatarBytes = guest.Avatar,
        //                              VisitPurpose = purpose.Name,
        //                              MeetingType = "Личное посещение",
        //                              Status = status.StatusName,
        //                              MeetingId = privateMeeting.Id
        //                              //PassportBytes = guest.Passport
        //                          }).ToListAsync();
        //    var r = await (from groupMeetingGuests in _dbContext.GroupMeetingsGuests
        //                   join guest in _dbContext.Guests on groupMeetingGuests.GuestId equals guest.IdGuests
        //                   join groupMeeting in _dbContext.GroupMeetings on groupMeetingGuests.GroupMeetingId equals groupMeeting.GroupMeetingId
        //                   join department in _dbContext.Departments on groupMeeting.DeprtmentId equals department.Id
        //                   join employee in _dbContext.Employees on groupMeeting.EmployeeId equals employee.IdEmployees
        //                   join status in _dbContext.MeetingStatuses on groupMeeting.StatusId equals status.IdStatus
        //                   join purpose in _dbContext.VisitPurposes on groupMeeting.VisitPurposeId equals purpose.IdVisitPurpose
        //                   where department.Id == departmentId
        //                   where status.IdStatus == 2
        //                   select new AllRequestInfo
        //                   {
        //                       Id = guest.IdGuests,
        //                       LastName = guest.LastName,
        //                       Name = guest.Name,
        //                       Patronymic = guest.Patronymic,
        //                       Phone = guest.Phone,
        //                       Email = guest.Email,
        //                       Organization = guest.Organization,
        //                       Note = guest.Note,
        //                       Birthday = guest.Birthday.ToString("yyyy-MM-dd"),
        //                       PassportSeries = guest.PassportSeries,
        //                       PassportNumber = guest.PassportNumber,
        //                       DateFrom = groupMeeting.DateFrom.ToString("yyyy-MM-dd"),
        //                       DateTo = groupMeeting.DateTo.ToString("yyyy-MM-dd"),
        //                       DateVisit = groupMeeting.DateVisit.ToString(),
        //                       Department = department.DepartmentName,
        //                       FullNameEmployee = employee.FullName,
        //                       AvatarBytes = guest.Avatar,
        //                       VisitPurpose = purpose.Name,
        //                       MeetingType = "Групповое посещение",
        //                       Status = status.StatusName,
        //                       MeetingId = groupMeeting.GroupMeetingId,
        //                       //PassportBytes = guest.Passport
        //                   }).ToListAsync();
        //    requests.AddRange(r);
        //    return requests;
        //}

        [HttpPut]
        public async Task SetEnterTime(AllowedRequest allowedRequest)
        {
            if (allowedRequest.MeetingType == "Личное посещение")
            {
                PrivateRequestsAllowedAccess request = await _dbContext.PrivateRequestsAllowedAccesses.Where(r => r.Id == allowedRequest.Id).FirstOrDefaultAsync();
                request.EnterTime = TimeOnly.Parse(allowedRequest.EnterTime);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                GroupRequestsAllowedAccess request = await _dbContext.GroupRequestsAllowedAccesses.Where(r => r.Id == allowedRequest.Id).FirstOrDefaultAsync();
                request.EnterTime = TimeOnly.Parse(allowedRequest.EnterTime);
                await _dbContext.SaveChangesAsync();
            }
        }


        //изменение пароля
        //[HttpPost]
        //public void ChangePassword([FromBody] ChangePassword value)
        //{
        //    User user = (from u in _dbContext.Users where u.Login == CurrentUser.login select u).First();
        //    user.Salt = BCrypt.Net.BCrypt.GenerateSalt();
        //    user.Password = BCrypt.Net.BCrypt.HashPassword(value.Password, user.Salt!);
        //    _dbContext.Update(user);
        //    _dbContext.SaveChanges();
        //}
        ////удаление пользователя
        //[HttpDelete]
        //public void DeleteUser()
        //{
        //    User user = (from u in _dbContext.Users where u.Login == CurrentUser.login select u).First();
        //    CurrentUser.login = "";
        //    _dbContext.Users.Remove(user);
        //    _dbContext.SaveChanges();
        //}
    }
}
