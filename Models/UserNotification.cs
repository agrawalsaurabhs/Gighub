using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class UserNotification
    {
        private readonly ApplicationUser _user;
        private readonly Notification _notificationn;

        [Key]
        [Column(Order = 2)]
        public int NotificationId { get; private set; }

        [Key]
        [Column(Order = 1)]
        public string UserId { get; private set; }

        public ApplicationUser User { get; private set; }
        public Notification Notification { get; private set; }
        public bool IsRead { get; set; }

        protected UserNotification()
        {

        }

        public UserNotification(ApplicationUser user, Notification notificationn)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (notificationn == null) throw new ArgumentNullException("notification");
            _user = user;
            _notificationn = notificationn;
        }
    }
}