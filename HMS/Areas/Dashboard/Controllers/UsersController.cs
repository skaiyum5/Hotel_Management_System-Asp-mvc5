using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
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
    public class UsersController : Controller
    {

        private HMSSignInManager _signInManager;
        private HMSUserManager _userManager;
        private HMSRolesManager _roleManager;


        public UsersController()
        {
        }

        public UsersController(HMSUserManager userManager, HMSSignInManager signInManager, HMSRolesManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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


        public ActionResult Index(string searchTerm, int? page, string roleID)
        {

            int recordSize = 3;
            page = page ?? 1;
            UsersListingModel model = new UsersListingModel();
            model.SearchTerm = searchTerm;
            model.RoleID = roleID;
            model.Roles = RoleManager.Roles.AsQueryable();
            model.UsersCount = SearchUsersCount(searchTerm, roleID);
            model.Users = SearchUsers(searchTerm,roleID, recordSize, page.Value);
            model.Pager = new Pager(model.UsersCount, page, recordSize);
            return View(model);
        }
        public IEnumerable<HMSUser> SearchUsers(string searchTerm, string roleID, int recordSize, int page)
        {
           
            var users = UserManager.Users.ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            //if (accommodationTypeID.HasValue)
            //{
            //    users = users.Where(a => a.roleID == accommodationTypeID.Value);
            //}
            var skip = (page - 1) * recordSize;
            return users.OrderBy(x => x.Email).Skip(skip).Take(recordSize).AsQueryable();

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
            UsersActionModel model = new UsersActionModel();
            model.Roles= RoleManager.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(ID)) //try to edit records
                {
                var user = await UserManager.FindByIdAsync(ID);
                model.ID = user.Id;
                model.FullName = user.FullName;
                model.UserName = user.UserName;
                model.Email = user.Email;
                model.Phone = user.Phone;
                model.Address = user.Address;
                model.City = user.City;
                model.Country = user.Country;
                }
            return PartialView("_Action", model);
        }
        [HttpPost]
        public async Task<JsonResult> Action(UsersActionModel user)
        {
            IdentityResult result = null;
            JsonResult json = new JsonResult();
            //HMSUser model = new HMSUser();

            if (!string.IsNullOrEmpty(user.ID)) //try to edit records
            {
                var model = await UserManager.FindByIdAsync(user.ID);
                model.Id = user.ID;
                model.FullName = user.FullName;
                model.UserName = user.UserName;
                model.Email = user.Email;
                model.Phone = user.Phone;
                model.Address = user.Address;
                model.City = user.City;
                model.Country = user.Country;
                result = await UserManager.UpdateAsync(model);

            }
            else
            {
                var model = new HMSUser();
                model.FullName = user.FullName;
                model.UserName = user.UserName;
                model.Email = user.Email;
                model.Phone = user.Phone;
                model.Address = user.Address;
                model.City = user.City;
                model.Country = user.Country;
                result = await UserManager.CreateAsync(model);

            }

           json.Data = new { Success = result.Succeeded, Message = string.Join(",", result.Errors) };
            return json;
        }
        [HttpGet]
        public async Task<ActionResult> UserRoles(string ID)
            {
            UsersRolesModel model = new UsersRolesModel();
           
            model.UserID = ID;
            var user = await UserManager.FindByIdAsync(ID);
            var userRoleIDs = user.Roles.Select(x=>x.RoleId).ToList();
            model.UserRoles = RoleManager.Roles.Where(x => userRoleIDs.Contains(x.Id)).ToList();
            model.Roles = RoleManager.Roles.Where(x =>!userRoleIDs.Contains(x.Id)).ToList();
            return PartialView("_UserRoles", model);
        }
        [HttpPost]
        public async Task<JsonResult> UserRoles(string userID, string roleID,bool isDelete=false)
        {
            IdentityResult result = null;
            JsonResult json = new JsonResult();
           // var user = await UserManager.FindByIdAsync(userID);
            var role = await RoleManager.FindByIdAsync(roleID);
            if (isDelete) 
            {
                result = await UserManager.RemoveFromRoleAsync(userID, role.Name);
            }
            else//try to Assign records
            {
                result = await UserManager.AddToRoleAsync(userID, role.Name);
            }

           json.Data = new { Success = result.Succeeded, Message = string.Join(",", result.Errors) };
            return json;
        }

        public ActionResult Delete(string ID)
        {
            UsersActionModel model = new UsersActionModel();
            model.ID = ID;
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(UsersActionModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID))
            {
                var user = await UserManager.FindByIdAsync(model.ID);
                result = await UserManager.DeleteAsync(user);
                json.Data = new { Success = result.Succeeded,Message=string.Join(",",result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to Delete User" };
            }
            return json;
        }
    }
   
}