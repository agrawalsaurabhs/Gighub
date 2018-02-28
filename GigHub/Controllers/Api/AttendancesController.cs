using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto attendanceDto)
        {
            var attendeId = User.Identity.GetUserId();
            if (_context.Attendences.Any(a => a.GigId == attendanceDto.GigId && a.AttendeId == attendeId))
            {
                return BadRequest("Attendance already exists");
            }

            var attendance = new Attendance()
            {
                GigId = attendanceDto.GigId,
                AttendeId = attendeId
            };
            _context.Attendences.Add(attendance);
            _context.SaveChanges();
            return Ok();
        }


        [HttpDelete]
        public IHttpActionResult DeleteAttendence(AttendanceDto attendanceDto)
        {
            var attendeId = User.Identity.GetUserId();
            var attendence = _context.Attendences.SingleOrDefault(a => a.GigId == attendanceDto.GigId && a.AttendeId == attendeId);

            if (attendence == null)
            {
                return NotFound();
            }

            _context.Attendences.Remove(attendence);
            _context.SaveChanges();
            return Ok();
        }
    }
}
