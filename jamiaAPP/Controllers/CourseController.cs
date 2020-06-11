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
    public class CourseController : Controller
    {
        private JamiaRDBEntities db;

        public CourseController()
        {
            this.db = new JamiaRDBEntities();
        }
        // GET: Course
        public ActionResult Index()
        {
            if (Session["UserName"] != null && Session["UserRole"].ToString() == "Admin")
            {
                Session["page"] = "Course Management";
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public JsonResult List()
        {
            var course_View = (from x in db.tbl_course select new { x.courseID, x.courseName, x.courseInstructor, x.courseDescription });



            return Json(course_View, JsonRequestBehavior.AllowGet);
        }

        // GET: Course/Details/5
        public JsonResult Details(int id)
        {
            var course = (from x in db.tbl_course where x.courseID==id select new { x.courseID, x.courseName, x.courseInstructor, x.courseDescription });


           

            return Json(course, JsonRequestBehavior.AllowGet);
        }

        
        // POST: Course/Create
        [HttpPost]
        public JsonResult Create(tbl_course course)
        {
            try{
                // TODO: Add insert logic here
                db.tbl_course.Add(course);
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

       

        // POST: Course/Edit/5
        
        public JsonResult Edit(int id, tbl_course course)
        {
            
            try
            {
                // TODO: Add update logic here
                var oldcourse = db.tbl_course.SingleOrDefault(x => x.courseID == id);
                oldcourse.courseName = course.courseName;
                oldcourse.courseInstructor = course.courseInstructor;
                oldcourse.courseDescription = course.courseDescription;
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }

        
        // POST: Course/Delete/5
        
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var delete_course = db.tbl_course.SingleOrDefault(x => x.courseID == id);
                db.tbl_course.Remove(delete_course);
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
