using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
   public class AccommodationPackagesServices
    {
        public static AccommodationPackagesServices Instance
        {
            get
            {
                if (instance == null)instance=new AccommodationPackagesServices();
                {
                    return instance;
                }
            }
        }
        private static AccommodationPackagesServices instance { get; set; }
        public AccommodationPackagesServices()
        {

        }
        public IEnumerable<AccommodationPackage> GetAccommodationPackages()
        {
            var context = new HMSContext();
            return context.AccommodationPackages.ToList();
        }
        public IEnumerable<AccommodationPackage> SearchAccommodationPackages(string searchTerm,int? accommodationTypeID ,int recordSize, int page)
        {
            var context = new HMSContext();
           
                var accommodationTypePackage = context.AccommodationPackages.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    accommodationTypePackage = accommodationTypePackage.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
                }

                if (accommodationTypeID.HasValue)
                {
                    accommodationTypePackage = accommodationTypePackage.Where(a => a.AccommodationTypeID==accommodationTypeID.Value);
                }
                var skip = (page - 1) * recordSize;
            return accommodationTypePackage.OrderBy(x=>x.Name).Skip(skip).Take(recordSize).ToList();
           
           // return accommodationTypePackage.ToList();
        }
        public int SearchAccommodationPackagesCount(string searchTerm,int? accommodationTypeID )
        {
            using (var context = new HMSContext())
            {
                var accommodationTypePackage = context.AccommodationPackages.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    accommodationTypePackage = accommodationTypePackage.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
                }

                if (accommodationTypeID.HasValue)
                {
                    accommodationTypePackage = accommodationTypePackage.Where(a => a.AccommodationTypeID == accommodationTypeID.Value);
                };
                return accommodationTypePackage.Count();
            }
        }

        public AccommodationPackage GetAccommodationPackageByID(int ID)
        {
            var context = new HMSContext();
            return context.AccommodationPackages.Find(ID);
        }

        public bool SaveAccommodationPackage(AccommodationPackage accommodationPackage)
        {
            var context = new HMSContext();
            context.AccommodationPackages.Add(accommodationPackage);
            return context.SaveChanges() > 0;
        }

        public bool UpdateAccommodationPackage(AccommodationPackage accommodationPackage)
        {
            var context = new HMSContext();
            var exitingAccommodationPackage = context.AccommodationPackages.Find(accommodationPackage.ID);
            context.AccommodationPackagePictures.RemoveRange(exitingAccommodationPackage.AccommodationPackagePictures);
            context.Entry(exitingAccommodationPackage).CurrentValues.SetValues(accommodationPackage); 
            context.AccommodationPackagePictures.AddRange(accommodationPackage.AccommodationPackagePictures);
            return context.SaveChanges() > 0;
        }

        public bool DeleteAccommodationPackage(AccommodationPackage accommodationPackage)
        {
            var context = new HMSContext();
            context.Entry(accommodationPackage).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
    }
}
