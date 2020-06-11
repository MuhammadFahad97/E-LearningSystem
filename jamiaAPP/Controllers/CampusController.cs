using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jamiaAPP.Models;
using jamiaAPP.viewModels;
using jamiaAPP.Filters;
namespace jamiaAPP.Controllers
{
    public class CampusController : Controller
    {
        private JamiaRDBEntities db;
        public CampusController()
        {

            this.db = new JamiaRDBEntities();
        }
        // GET: Campus
        [RoleBasedAuthorize]
        public ActionResult Index()
        {
            Session["page"] = "Campus Management";
            return View();
            //if (Session["UserName"] != null && Session["UserRole"].ToString() == "Admin")
            //{
            //    return View();
            //}
            //else return RedirectToAction("Index","Home");
        }


        //get all
        [HttpGet]
        [RoleBasedAuthorize]
        public JsonResult List()
        {
            var campus_View = (from x in db.tbl_campus select new { x.campusID,x.campusName,x.campusAddress,x.campusDescription}) ;
                                            
                        
           
            return Json(campus_View,JsonRequestBehavior.AllowGet);
        }



        // GET: Campus/Details/5
        [HttpGet]
        [RoleBasedAuthorize]
        public JsonResult Details(int id)
        {
            var campus= (from x in db.tbl_campus where x.campusID==id select new { x.campusID, x.campusName, x.campusAddress, x.campusDescription });

            return Json(campus, JsonRequestBehavior.AllowGet);
        }

       
        // POST: Campus/Create
        [HttpPost]
        [RoleBasedAuthorize]
        public JsonResult Create(tbl_campus campus)
        {
            try
            {
                // TODO: Add insert logic here
                db.tbl_campus.Add(campus);
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }


        // POST: Campus/Edit/5
        [RoleBasedAuthorize]
        public JsonResult Edit(int id, tbl_campus campus)
        {
            try
            {
                // TODO: Add update logic here
              var oldcampus =  db.tbl_campus.SingleOrDefault(x => x.campusID == id);
                oldcampus.campusName = campus.campusName;
                oldcampus.campusAddress = campus.campusAddress;
                oldcampus.campusDescription = campus.campusDescription;
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
               
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }


        // POST: Campus/Delete/5
        [RoleBasedAuthorize]
        public JsonResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var delete_campus = db.tbl_campus.SingleOrDefault(x => x.campusID == id);
                db.tbl_campus.Remove(delete_campus);
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Could Not Delete Campus!!, First Delete The Records Which Dependents On Campus ", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
