using Microsoft.AspNetCore.Mvc;
using PowerStore.Core.Contract.Errors;
using PowerStore.Core.Contract.IdentityInterface;
using PowerStore.Core.Entities;

namespace PowerStore.APIs.Controllers
{

    public class AdminController : BaseApiController
    {
        private readonly IDriverService _driverService;

        public AdminController(
             IDriverService driverService
            )
        {
            _driverService = driverService;
        }

        [HttpGet("drivers/{status}")]
        public ActionResult<IEnumerable<Driver>> GetDrivers(DriverStatusWork status)
        {
            var pendingDrivers = _driverService.GetBy(e => e.StatusWork == status);
            return Ok(pendingDrivers.ToList());
        }

        [HttpGet("drivers/Pending")]
        public ActionResult<IEnumerable<Driver>> GetPendingDrivers()
        {
            var pendingDrivers = _driverService.GetBy(e => e.StatusWork == DriverStatusWork.Pending);
            return Ok(pendingDrivers.ToList());
        }
        [HttpGet("drivers/approved")]
        public ActionResult<IEnumerable<Driver>> GetApprovedDrivers()
        {
            var approvedDrivers = _driverService.GetBy(e => e.StatusWork == DriverStatusWork.Approved);

            return Ok(approvedDrivers.ToList());
        }

        [HttpGet("drviers/changeStatus/{id}/{status}")]
        public ActionResult<string> ApproveDriver(string id, DriverStatusWork status)
        {
            var driver = _driverService.GetBy(x => x.UserId == id).FirstOrDefault();
            if (driver == null)
            {
                return BadRequest(new ApiResponse(400, "The Drive Is Not Exist."));
            }

            driver.StatusWork = status;
            _driverService.Update(driver);
            return Ok("Driver approved successfully.");
        }


        [HttpGet("drviers/approve/{id}")]
        public ActionResult<string> ApproveDriver(string id)
        {
            var driver = _driverService.GetBy(x => x.UserId == id).FirstOrDefault();
            if (driver == null)
            {
                return BadRequest(new ApiResponse(400, "The Drive Is Not Exist."));
            }

            driver.StatusWork = DriverStatusWork.Approved;

            _driverService.Update(driver);
            return Ok("Driver approved successfully.");
        }
        [HttpPost("drivers/reject/{id}")]
        public IActionResult RejectDriver(string id)
        {
            var driver = _driverService.GetBy(x => x.UserId == id).FirstOrDefault();
            if (driver == null)
            {
                return BadRequest(new ApiResponse(400, "The Drive Is Not Exist."));
            }


            driver.StatusWork = DriverStatusWork.Rejected;

            _driverService.Update(driver);

            return Ok("Driver rejected.");
        }

    }
}
