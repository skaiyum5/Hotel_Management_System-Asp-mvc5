using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccommodationTypesServices
    {
        #region singleTon
        public static AccommodationTypesServices Instance
        {
            get {
                if (instance == null) instance = new AccommodationTypesServices();
                {
                    return instance;
                }

                }
        }
        private static AccommodationTypesServices  instance { get; set; }
        public AccommodationTypesServices()
        {

        }
        #endregion
        public IEnumerable<AccommodationType> GetAllAccommodationType()
        {
            //using (var context = new HMSContext())
            //{
            //    return context.AccommodationTypes.ToList();

            //}
            var context = new HMSContext();

            return context.AccommodationTypes.AsEnumerable();

        }
        public IEnumerable<AccommodationType> SearchAccommodationType(string searchTerm)
        {
            var context = new HMSContext();
            var accommodationTypes = context.AccommodationTypes.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accommodationTypes =accommodationTypes.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            return accommodationTypes.AsQueryable();

        }

        public AccommodationType GetAccommodationByID(int ID)
        {
            var context = new HMSContext();

            return context.AccommodationTypes.Find(ID);
        }

        public bool SaveAccommodationTypes(AccommodationType accommodationType)
        {
            var result =1;
            using (var context = new HMSContext())
            {
                context.AccommodationTypes.Add(accommodationType);
                result=  context.SaveChanges();
            }
            return result > 0;
        }

        public bool UpdateAccommodationType(AccommodationType model)
        {
            var context=new HMSContext();
            context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges()>0;
        }

        public bool DeleteAccommodation(AccommodationType model)
        {
            var context = new HMSContext();
            context.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }
    }
}
