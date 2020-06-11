using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jamiaAPP.Filters;
using jamiaAPP.Models;

namespace jamiaAPP.Controllers
{
    [RoleBasedAuthorize]
    public class BatchCourseController : Controller
    {
        private JamiaRDBEntities db;
        public BatchCourseController()
        {
            this.db = new JamiaRDBEntities();
        }
        // GET: BatchCourse
        public ActionResult Index()
        {
            if (Session["UserName"] != null && Session["UserRole"].ToString() == "Admin")
            {
                Session["page"] = "Batch-Course Management";
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public JsonResult List()
        {
            //many to many query 
             var course_View = (from x in db.tbl_course
                                from y in x.tbl_batch
                                select new { x.courseID, x.courseName,x.courseInstructor, y.batchID, y.batchDescription });
           

            return Json(course_View, JsonRequestBehavior.AllowGet);
        }


        
        // POST: BatchCourse/Create
        [HttpPost]
        public ActionResult Create(int batchid, int courseid)
        {
            try
            {

                

                var  course_View = (from x in db.tbl_course
                                   from y in x.tbl_batch
                                   where x.courseID == courseid && y.batchID == batchid
                                   select new { x.courseID, x.courseName, x.courseInstructor, y.batchID, y.batchDescription });
                // TODO: Add insert logic here

                    if (course_View.Count()<1) { 

                    var create_course = db.tbl_course.SingleOrDefault(x => x.courseID == courseid);
                    var create_batch = db.tbl_batch.SingleOrDefault(x => x.batchID == batchid);
                    create_course.tbl_batch.Add(create_batch);
                    db.SaveChanges();
                    return Json("success", JsonRequestBehavior.AllowGet);

                    }
                return Json("failure to assign or is being reassigned", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("failure to assign or is being reassigned", JsonRequestBehavior.AllowGet);
            }
        }

        
        // POST: BatchCourse/Delete/5
        [HttpPost]
        public ActionResult Delete(int batchid,int courseid )
        {
            try
            {
                
                // TODO: Add delete logic here
                var delete_course = db.tbl_course.SingleOrDefault(x => x.courseID == courseid);
                var delete_batch = db.tbl_batch.SingleOrDefault(x => x.batchID == batchid);
                delete_course.tbl_batch.Remove(delete_batch);
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
