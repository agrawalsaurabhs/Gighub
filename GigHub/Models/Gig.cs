using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Models
{
    public class Gig
    {

        public Gig()
        {
            Attendences = new Collection<Attendance>();
        }
        public int Id { get; set; }

        [Required]
        public string ArtistId { get; set; }

        public ApplicationUser Artist { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        public Genre Genre { get; set; }

        [Required]
        public byte GenreId { get; set; }

        public bool IsCancelled { get; private set; }

        public ICollection<Attendance> Attendences { get; private set; }

        //Saurabh, don't pass context here, its bad design
        public void Cancel(ApplicationDbContext context)
        {
            IsCancelled = true;

            var notification = Notification.GigCreated(this);
            context.Notifications.Add(notification);

            Attendences.Select(a => a.Attende).ToList().ForEach(a => a.Notify(notification));

        }

        //Saurabh, don't pass context here, its bad design
        public void Update(ApplicationDbContext context, DateTime dateTime, byte genreid, string venue)
        {
            var notification = Notification.GigUpdated(this, this.DateTime, this.Venue);

            this.DateTime = dateTime;
            this.GenreId = genreid;
            this.Venue = venue;

            context.Notifications.Add(notification);
            Attendences.Select(a => a.Attende).ToList().ForEach(a => a.Notify(notification));
        }

        //Saurabh : Instead of attendences we need following of the creator
        public void Create(ApplicationDbContext context, Gig gig)
        {
            var notification = Notification.GigCreated(gig);
            context.Gigs.Add(gig);
            context.Notifications.Add(notification);
            Attendences.Select(a => a.Attende).ToList().ForEach(a => a.Notify(notification));
        }
    }
}