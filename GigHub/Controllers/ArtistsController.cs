using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistsController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Following()
        {
            var userid = User.Identity.GetUserId();
            var artists = _context.Followings.Where(f => f.FollowerId == userid).Include(f => f.Followee).ToList();
            return View(artists);

        }
    }
}