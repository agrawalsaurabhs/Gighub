using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }
        public string OriginalVenue { get; private set; }

        private Notification(Gig gig, NotificationType notificationType)
        {
            if (gig == null)
            {
                throw new ArgumentNullException("Gig is null");
            }

            this.Gig = gig;
            this.NotificationType = notificationType;
            this.DateTime = DateTime.Now;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(gig, NotificationType.GigCreated);
        }

        public static Notification GigUpdated(Gig gig, DateTime originalDateTime, string originalVenue)
        {
            var notification = new Notification(gig, NotificationType.GigUpdated)
            {
                OriginalDateTime = originalDateTime,
                OriginalVenue = originalVenue
            };
            return notification;
        }

        public Notification()
        {

        }

        [Required]
        public Gig Gig { get; private set; }
    }
}