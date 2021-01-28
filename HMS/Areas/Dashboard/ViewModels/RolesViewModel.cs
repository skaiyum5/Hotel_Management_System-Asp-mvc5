using HMS.Entities;
using HMS.ViewModel;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class RolesViewModel
    {
    }
    public class RolesListingModel
    {
        public string UserID { get; set; }
        public IEnumerable<HMSUser> Users { get; set; }
        public string SearchTerm { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public string RoleID { get; set; }
        public Pager Pager { get; set; }
        public int UsersCount { get; set; }
    }
    public class RolesActionModel
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}