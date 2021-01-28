using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccommodationViewModel
    {

    }
    public class AccommodationListingModel
    {
        public string SearchTerm { get; set; }
        public IEnumerable<Accommodation> Accommodations { get; set; }
    }
    public class AccommodationActionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}