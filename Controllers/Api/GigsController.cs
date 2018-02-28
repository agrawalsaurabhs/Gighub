using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            string userid = User.Identity.GetUserId();
            var gig = _context.Gigs.Include(g => g.Attendences.Select(a => a.Attende)).
                Single(g => g.Id == id && g.ArtistId == userid);

            if (gig.IsCancelled)
            {
                return NotFound();
            }

            gig.Cancel(_context);
           
            _context.SaveChanges();
            return Ok();
        }
    }
}
