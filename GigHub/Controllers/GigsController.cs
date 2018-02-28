using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userid = User.Identity.GetUserId();
            var gigs = _context.Gigs.Where(g => g.ArtistId == userid &&
                                                g.DateTime > DateTime.Now && g.IsCancelled == false)
                                                .Include(g => g.Genre).ToList();
            return View(gigs);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userid = User.Identity.GetUserId();
            var gigs = _context.Attendences.Where(a => a.AttendeId == userid)
                .Select(a => a.Gig).Include(a => a.Artist).Include(g => g.Genre).ToList();

            var attendances = _context.Attendences.Where(a => a.AttendeId == userid
                                                             && a.Gig.DateTime > DateTime.Now)
                                                             .ToList()
                                                             .ToLookup(a => a.GigId);

            GigsViewModel vm = new GigsViewModel
            {
                UpComingGigs = gigs,
                Attendances = attendances
            };

            return View("Gigs", vm);
        }

        [Authorize]
        public ActionResult Create()
        {
            var vm = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Heading = "Add a gig"
            };
            return View("GigForm", vm);
        }

        [Authorize]
        public ActionResult Edit(int gigid)
        {
            var userid = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == gigid && g.ArtistId == userid);
            var vm = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Id = gig.Id,
                Venue = gig.Venue,
                Genre = gig.GenreId,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Heading = "Edit a gig"
            };
            return View("GigForm", vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };
            gig.Create(_context, gig);
            _context.SaveChanges();
            return RedirectToAction("Mine");
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            string userid = User.Identity.GetUserId();
            var gig = _context.Gigs.Include(g => g.Attendences.Select(a => a.Attende)).Single(g => g.Id == viewModel.Id && g.ArtistId == userid);
            gig.Update(_context, viewModel.GetDateTime(), viewModel.Genre, viewModel.Venue);
            _context.SaveChanges();
            return RedirectToAction("Mine");
        }

        [HttpPost]
        public ActionResult Search(string searchterm)
        {
            return RedirectToAction("Index", "Home", new { query = searchterm });
        }
    }
}