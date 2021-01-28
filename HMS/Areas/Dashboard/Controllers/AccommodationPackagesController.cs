using HMS.Areas.Dashboard.ViewModels;
using HMS.Entities;
using HMS.Services;
using HMS.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccommodationPackagesController : Controller
    {
        // GET: Dashboard/AccommodationPackages
        
        public ActionResult Index(string searchTerm,int? page,int? accomodationTypeID)
        {
            int recordSize = 5;
            page = page ?? 1;
            AccommodationPackageListingModel model = new AccommodationPackageListingModel();
            model.SearchTerm = searchTerm;
            model.AccommodationTypeID = accomodationTypeID;
            model.AccommodationTypes = AccommodationTypesServices.Instance.GetAllAccommodationType();
            //model.AccommodationPackages = AccommodationPackagesServices.Instance.GetAccommodationPackages(); 
            model.AccommodationPackageCount= AccommodationPackagesServices.Instance.SearchAccommodationPackagesCount(searchTerm,accomodationTypeID);
            model.AccommodationPackages = AccommodationPackagesServices.Instance.SearchAccommodationPackages(searchTerm, accomodationTypeID, recordSize, page.Value);
            model.Pager = new Pager(model.AccommodationPackageCount, page, recordSize);
            return View(model);
        }
        public ActionResult Action(int? ID)
        {
            AccommodationPackageModel model = new AccommodationPackageModel();
            model.AccommodationTypes = AccommodationTypesServices.Instance.GetAllAccommodationType();
            if (ID.HasValue)
            {
                var accommodationType = AccommodationPackagesServices.Instance.GetAccommodationPackageByID(ID.Value) ;
                model.ID = accommodationType.ID;
                model.Name = accommodationType.Name;
                model.FeePerNight = accommodationType.FeePerNight;
                model.NoOfRoom = accommodationType.NoOfRoom;
                model.AccommodationTypeID = accommodationType.AccommodationTypeID;
                model.AccommodationPackagePictures = accommodationType.AccommodationPackagePictures;
            }
            return PartialView("_Action",model);
        }
        [HttpPost]
        public JsonResult Action(AccommodationPackageModel model)
        {
            var result = false;
            JsonResult json = new JsonResult();
            SharedServices sharedServices = new SharedServices();

            
            List<int> pictureIDs = !string.IsNullOrEmpty(model.PictureIDs)? model.PictureIDs.Split(',').Select(x => int.Parse(x)).ToList():new List<int>();
            var pictures = sharedServices.GetPictureByID(pictureIDs);
             if (model.ID>0)//for Edit
            {
                var accommodationPackage = AccommodationPackagesServices.Instance.GetAccommodationPackageByID(model.ID);
                accommodationPackage.AccommodationTypeID = model.AccommodationTypeID;
                accommodationPackage.Name = model.Name;
                accommodationPackage.FeePerNight = model.FeePerNight;
                accommodationPackage.NoOfRoom = model.NoOfRoom;
                
                accommodationPackage.AccommodationPackagePictures.Clear();
                accommodationPackage.AccommodationPackagePictures.AddRange(pictures.Select(x => new AccommodationPackagePicture() { AccommodationPackageID = model.ID, PictureID = x.ID }));

                result =  AccommodationPackagesServices.Instance.UpdateAccommodationPackage(accommodationPackage);
            }
            else//for create
            {
                AccommodationPackage accommodationPackage = new AccommodationPackage();

                accommodationPackage.AccommodationTypeID = model.AccommodationTypeID;
                accommodationPackage.Name = model.Name;
                accommodationPackage.FeePerNight = model.FeePerNight;
                accommodationPackage.NoOfRoom = model.NoOfRoom;
                accommodationPackage.AccommodationPackagePictures = new List<AccommodationPackagePicture>();
                accommodationPackage.AccommodationPackagePictures.AddRange(pictures.Select(x => new AccommodationPackagePicture() { PictureID = x.ID }));

                result = AccommodationPackagesServices.Instance.SaveAccommodationPackage(accommodationPackage);
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
            AccommodationPackageModel model = new AccommodationPackageModel();
            model.ID= ID;
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public JsonResult Delete(AccommodationPackageModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;

            AccommodationPackage accommodationPackage = new AccommodationPackage();
            accommodationPackage.ID = model.ID;
            result = AccommodationPackagesServices.Instance.DeleteAccommodationPackage(accommodationPackage);
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
    }
}