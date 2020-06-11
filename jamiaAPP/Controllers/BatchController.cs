using jamiaAPP.Filters;
using jamiaAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jamiaAPP.Controllers
{
    [RoleBasedAuthorize]
    public class BatchController : Controller
    {
        private JamiaRDBEntities db;
        public BatchController()
        {
            this.db = new JamiaRDBEntities();

        }
        // GET: Batch
        public ActionResult Index()
        {
            if (Session["UserName"] != null && Session["UserRole"].ToString() == "Admin")
            {
                Session["page"] = "Batch Management";
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public JsonResult List()
        {
            var batch_View = (from x in db.tbl_batch select new { x.batchID, x.batchDescription });



            return Json(batch_View, JsonRequestBehavior.AllowGet);
        }

        // GET: Batch/Details/5
        public JsonResult Details(int id)
        {
            var course = (from x in db.tbl_batch where x.batchID == id select new { x.batchID, x.batchDescription });
            
            //gives an array
            return Json(course, JsonRequestBehavior.AllowGet);
        }

        // GET: Batch/Create
       
        // POST: Batch/Create
        [HttpPost]
        public ActionResult Create(tbl_batch batch)
        {
            try
            {
                // TODO: Add insert logic here
                if (db.tbl_batch.SingleOrDefault(x => x.batchID == batch.batchID) == null)
                {

                    db.tbl_batch.Add(batch);
                    db.SaveChanges();
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else return Json("batch number is already taken select another", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Batch/Edit/5
        
        // POST: Batch/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, tbl_batch batch)
        {
            try
            {
                // TODO: Add update logic here
                var oldbatch = db.tbl_batch.SingleOrDefault(x => x.batchID == id);
                if (oldbatch != null)
                {
                    
                    oldbatch.batchDescription = batch.batchDescription;

                    db.SaveChanges();

                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else return Json("batch not found", JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json("failure", JsonRequestBehavior.AllowGet);
            }

        }

        // GET: Batch/Delete/5
        
        // POST: Batch/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var delete_batch = db.tbl_batch.SingleOrDefault(x => x.batchID == id);
                db.tbl_batch.Remove(delete_batch);
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }

        }
    }
}
