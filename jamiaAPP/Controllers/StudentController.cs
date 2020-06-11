using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jamiaAPP.Models;
using jamiaAPP.viewModels;
using Newtonsoft.Json;

namespace jamiaAPP.Controllers
{
    public class StudentController : Controller
    {
        private JamiaRDBEntities db;
        
        public StudentController()
        {
            this.db = new JamiaRDBEntities();
        }

        // GET: Student
        public ActionResult Index()
        {
            Session["page"] = "Student Portal";
            return View();
        }

        public JsonResult GetStudentCourses() {
            int userid = Convert.ToInt32(Session["UserID"]);

            var student = (from u in db.tbl_user
                           join c in db.tbl_campus on u.CID equals c.campusID
                           join b in db.tbl_batch on u.BID equals b.batchID
                           where u.userID == userid
                           select new
                           {

                               u.userID,
                               u.userName,
                               u.userPassword,
                               u.userRole,
                               u.userDisplayName,
                               u.userCellNumber,

                               c.campusName,
                               b.batchID,


                           });


            var user = db.tbl_user.SingleOrDefault(x => x.userID == userid);

            var courses = (from b in db.tbl_batch
                           from c in b.tbl_course
                           where b.batchID == user.BID
                           select new
                           {
                               c.courseID,
                               c.courseName,
                               c.courseInstructor,
                               c.courseDescription,


                           });


            
            return Json(new {student,courses }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult getfiles(string coursename) {

            try {
                var courseid = db.tbl_course.SingleOrDefault(x => x.courseName == coursename);
                var files = (from x in db.tbl_files
                                  join y in db.tbl_course on x.fileCourse equals y.courseID
                                  where x.fileCourse==courseid.courseID
                                  select new { x.fileID, x.fileDispName, x.fileCourse, x.filePath, x.fileType, x.fileDescription, y.courseName });
                return Json(files, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult DownloadFile(string filename)
        {
            try
            {
                var file = db.tbl_files.FirstOrDefault(x => x.fileDispName == filename);

                byte[] fileBytes = System.IO.File.ReadAllBytes(file.filePath);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.fileDispName);
            }
            catch (Exception ex)
            {

                return Json("Unable to Download File PLease Retry ERROR : " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }

   
}