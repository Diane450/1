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

        [HttpGet]
        public async Task<List<AllRequestInfo>> GetAllRequests()
        {
            return null;
        }
    }
}
