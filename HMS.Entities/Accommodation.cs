using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entities
{
    public class Accommodation
    {
        public int ID { get; set; }
        public virtual AccommodationPackage AccommodationPackage { get; set; }
        public int AccommodationPackageID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<AccommodationPicture> AccommodationPictures { get; set; }
    }
}
