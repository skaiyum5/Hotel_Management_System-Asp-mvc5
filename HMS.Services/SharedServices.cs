using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
   public class SharedServices
    {
        public bool SavePictures(Picture picture)
        {
            var context = new HMSContext();
            context.Pictures.Add(picture);
            return context.SaveChanges()>0;
                
        }
        public List<Picture> GetPictureByID(List<int> pictureIDs)
        {
            var context = new HMSContext();
           
            return pictureIDs.Select(id=>context.Pictures.Find(id)).ToList();
                
        }
    }
}
