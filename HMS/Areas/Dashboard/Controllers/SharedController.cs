using HMS.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HMS.Services;

namespace HMS.Areas.Dashboard.Controllers
{
    public class SharedController : Controller
    {
        // GET: Dashboard/Shared
        public JsonResult UploadImage()
        {
            JsonResult result = new JsonResult();
            //result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var PicList = new List<Picture>();
            SharedServices sharedServices = new SharedServices();
            try
            {
                var files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    var pictures = files[i];
                    var fileName = Guid.NewGuid() + Path.GetExtension(pictures.FileName);
                    var path = Path.Combine(Server.MapPath("/images/site/"), fileName);
                    pictures.SaveAs(path);

                    var dbPicture = new Picture();
                    dbPicture.Url = fileName;

                    if (sharedServices.SavePictures(dbPicture))
                    {
                      PicList.Add(dbPicture);
                    } 

                    
                }
                    // result.Data = new {Success = true, ImageUrl = string.Format("/Content/images/{0}", fileName)};
                        result.Data = new { Success = true, PicList };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = ex.Message };
            }

            return result;
        }
    }
}
