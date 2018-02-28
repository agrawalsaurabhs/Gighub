using AutoMapper;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using WebGrease.Css.Extensions;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userid = User.Identity.GetUserId();
            var notifications = _context.UserNotifications.Where(u => u.UserId == userid && u.IsRead == false)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist).ToList();
            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var user = User.Identity.GetUserId();
            _context.UserNotifications.
                Where(un => un.UserId == user && un.IsRead == false).
                ForEach(un => un.IsRead = true);
            _context.SaveChanges();
            return Ok();
        }
    }
}
