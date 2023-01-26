using LicensingAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Runtime.InteropServices;

namespace LicensingAPI.Controllers
{
    [ApiController]
    [Route("api/license")]
    public class LicenseController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public LicenseController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }   

        [HttpGet]
        public IActionResult GetLicenses()
        {
            return Ok(_dbContext.Licenses.ToList());
        }
    }
}
