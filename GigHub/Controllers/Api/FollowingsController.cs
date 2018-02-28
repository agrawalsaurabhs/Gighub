using System.Linq;
using System.Web.Http;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [System.Web.Http.Authorize]
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }



        [System.Web.Http.HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userid = User.Identity.GetUserId();

            if (_context.Followings.Any(f => f.FollowerId == userid &&
                                             f.FolloweeId == dto.FolloweeeId))
            {
                return BadRequest("Following already exists");
            }
            var following = new Following()
            {
                FollowerId = userid,
                FolloweeId = dto.FolloweeeId
            };

            _context.Followings.Add(following);
            _context.SaveChanges();
            return Ok();
        }
    }
}
