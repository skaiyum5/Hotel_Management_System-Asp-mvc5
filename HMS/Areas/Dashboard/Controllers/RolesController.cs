using HMS.Areas.Dashboard.ViewModels;
using HMS.Services;
using HMS.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class RolesController : Controller
    {

        private HMSSignInManager _signInManager;
        private HMSUserManager _userManager;
        private HMSRolesManager _roleManager;

        public RolesController()
        {
        }

        public RolesController(HMSUserManager userManager, HMSSignInManager signInManager, HMSRolesManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public HMSSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<HMSSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public HMSUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<HMSUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public HMSRolesManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<HMSRolesManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Dashboard/Roles
        public ActionResult Index(string searchTerm, int? page)
        {

            int recordSize = 1;
            page = page ?? 1;
            RolesListingModel model = new RolesListingModel();
            model.SearchTerm = searchTerm;
            model.Roles = RoleManager.Roles.ToList();
            //model.UsersCount = SearchUsersCount(searchTerm, roleID);
           // model.Users = UserManager.Users;
           // model.Pager = new Pager(model.Users.Count(), page, recordSize);
            return View(model);
        }
        public IEnumerable<IdentityRole> SearchUsers(string searchTerm, string roleID, int recordSize, int page)
        {

            var roles = RoleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var skip = (page - 1) * recordSize;
            return roles.OrderBy(x=>x.Id).Skip(skip).Take(recordSize).ToList();

        }
        public int SearchUsersCount(string searchTerm, string roleID)
        {

            var users = UserManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            //if (accommodationTypeID.HasValue)
            //{
            //    users = users.Where(a => a.roleID == accommodationTypeID.Value);
            //}
            return users.Count();
        }


        public async Task<ActionResult> Action(string ID)
        {
            RolesActionModel model = new RolesActionModel();
            
            if (!string.IsNullOrEmpty(ID)) //try to edit records
            {
                var user = await RoleManager.FindByIdAsync(ID);
                model.ID = user.Id;
                model.Name = user.Name;
            }
            return PartialView("_Action", model);
        }
        [HttpPost]
        public async Task<JsonResult> Action(RolesActionModel role)
        {
            IdentityResult result = null;
            JsonResult json = new JsonResult();
            //HMSUser model = new HMSUser();

            if (!string.IsNullOrEmpty(role.ID)) //try to edit records
            {
                var model = await RoleManager.FindByIdAsync(role.ID);
                model.Id = role.ID;
                model.Name = role.Name;
                result = await RoleManager.UpdateAsync(model);
               
            }
            else
            {
                var model = new IdentityRole();
                model.Name = role.Name;
                result = await RoleManager.CreateAsync(model);
            }

            json.Data = new { Success = result.Succeeded, Message = string.Join(",", result.Errors) };
            return json;
        }

        public ActionResult Delete(string ID)
        {
            RolesActionModel model = new RolesActionModel();
            model.ID = ID;
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(RolesActionModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;

            //IdentityRole roles = new IdentityRole();
            //roles.Id = model.ID;
           
            if (!string.IsNullOrEmpty(model.ID))
            {
                var user = await RoleManager.FindByIdAsync(model.ID);
                result = await RoleManager.DeleteAsync(user);
                json.Data = new { Success = result.Succeeded, Message=string.Join(",",result.Errors) };
            }
            else
            {
                json.Data = new { success = false, Message = "Invalid User" };
            }
            return json;
        }
    }
}