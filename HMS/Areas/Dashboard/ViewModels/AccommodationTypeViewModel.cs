using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccommodationTypeListingModel
    {
        public string SearchTerm { get; set; }
        public IEnumerable<AccommodationType> AccommodationTypes { get; set; }
    }
    public class AccommodationTypeActionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}