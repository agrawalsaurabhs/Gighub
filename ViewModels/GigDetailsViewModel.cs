using System;

namespace GigHub.ViewModels
{
    public class GigDetailsViewModel
    {
        public string ArtistName { get; set; }
        public string Venue { get; set; }
        public DateTime DateTime { get; set; }
        public bool Going { get; set; }
        public bool Following { get; set; }

    }
}