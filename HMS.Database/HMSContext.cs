﻿using HMS.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Data
{
    public class HMSContext : IdentityDbContext<HMSUser>
    {
        public HMSContext() : base("HMSConnectionString")
        {
        }

        public static HMSContext Create()
        {
            return new HMSContext();
        }
        public DbSet<AccommodationType> AccommodationTypes { get; set; }
        public DbSet<AccommodationPackage> AccommodationPackages { get; set; }
        public DbSet<AccommodationPackagePicture> AccommodationPackagePictures { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Picture> Pictures { get; set; }

    }
}
