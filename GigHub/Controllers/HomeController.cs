using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _context.Gigs.
                Include(g => g.Artist).
                Include(g => g.Genre).
                Where(g => g.DateTime > DateTime.Now && g.IsCancelled == false);

            if (!string.IsNullOrEmpty(query))
            {
                upcomingGigs = upcomingGigs.Where(g => g.Artist.Name.Contains(query)
                                                       || g.Genre.Name.Contains(query) ||
                                                       g.Venue.Contains(query));
            }

            string userid = User.Identity.GetUserId();
            var attendances = _context.Attendences.Where(a => a.AttendeId == userid
                                                              && a.Gig.DateTime > DateTime.Now)
                                                              .ToList()
                                                              .ToLookup(a => a.GigId);

            var vm = new GigsViewModel()
            {
                UpComingGigs = upcomingGigs,
                Attendances = attendances
            };



            return View("Gigs", vm);
        }

        public ActionResult Details(int Id)
        {
            var user = User.Identity.GetUserId();
            var gig = _context.Gigs.Include(g => g.Artist)
                .SingleOrDefault(g => g.Id == Id);

            if (gig == null)
            {
                return HttpNotFound();
            }

            GigDetailsViewModel vm = new GigDetailsViewModel
            {
                DateTime = gig.DateTime,
                ArtistName = gig.Artist.Name,
                Venue = gig.Venue,
                Following = _context.Followings.Any(f => f.FollowerId == user
                && f.FolloweeId == gig.ArtistId),
                Going = _context.Attendences.Any(a => a.GigId == gig.Id
                && a.AttendeId == user)
            };

            return View("Details", vm);
        }
    }

    public class GigsViewModel
    {
        public IEnumerable<Gig> UpComingGigs { get; set; }
        public ILookup<int, Attendance> Attendances { get; set; }
    }
}