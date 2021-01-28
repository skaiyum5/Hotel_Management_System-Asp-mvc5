using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccommodationTypesController : Controller
    {
        // GET: Dashboard/AccommodationTypes
        public ActionResult Index(string searchTerm)
        {
            AccommodationTypeListingModel model = new AccommodationTypeListingModel();
            model.AccommodationTypes =AccommodationTypesServices.Instance.SearchAccommodationType(searchTerm);
            model.SearchTerm = searchTerm;
          
            return View(model);
        } 
        public ActionResult Action(int? ID)
        {
            AccommodationTypeActionModel model = new AccommodationTypeActionModel();
            if (ID.HasValue)
            {
              var accommodationType =  AccommodationTypesServices.Instance.GetAccommodationByID(ID.Value);
                model.ID = accommodationType.ID;
                model.Name = accommodationType.Name;
                model.Description = accommodationType.Description;
            }
           
            return PartialView("_Action", model);
        }
        [HttpPost]
        public JsonResult Action(AccommodationTypeActionModel model)
        {
            var result = false;
            JsonResult json = new JsonResult();
            AccommodationType accommodationType = new AccommodationType();
            accommodationType.ID = model.ID;
            accommodationType.Name = model.Name;
            accommodationType.Description = model.Description;
            if (model.ID>0)//for Edit item.
            {
               result = AccommodationTypesServices.Instance.UpdateAccommodationType(accommodationType);
            }
            else
            {
                result = AccommodationTypesServices.Instance.SaveAccommodationTypes(accommodationType);
            }
 
            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { success = false, Message = "Unable to add Accommodationtype" };
            }
            return json;
        }

        public ActionResult Delete(int ID)
        {

            AccommodationTypeActionModel model = new AccommodationTypeActionModel();
            model.ID = ID;
            return PartialView("_Delete",model );
        }
        [HttpPost]
        public JsonResult Delete(AccommodationTypeActionModel model)
        {
            var result = false;
            JsonResult json = new JsonResult();

            AccommodationType accommodationType = new AccommodationType();
            accommodationType.ID = model.ID;
            result = AccommodationTypesServices.Instance.DeleteAccommodation(accommodationType);
            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { success = false, Message = "Unable to delete Accommodationtype" };
            }
            //return RedirectToAction("Index");
            return json;
        }
    }
}