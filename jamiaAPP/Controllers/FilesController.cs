using jamiaAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using jamiaAPP.viewModels;
using jamiaAPP.Filters;
namespace jamiaAPP.Controllers
{[RoleBasedAuthorize]
    public class FilesController : Controller
    {
        private JamiaRDBEntities db;
        // GET: Files
        
        public FilesController()
        {
            this.db = new JamiaRDBEntities();
        }

        public ActionResult Index()
        {
            if (Session["UserName"] != null && Session["UserRole"].ToString() == "Admin")
            {
                Session["page"] = "Files Management";
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public JsonResult List()
        {
            var files_View = (from x in db.tbl_files join y in  db.tbl_course on x.fileCourse equals y.courseID
                              select new { x.fileID, x.fileDispName, x.filePath, x.fileType,x.fileDescription ,y.courseName});



            return Json(files_View, JsonRequestBehavior.AllowGet);
        }
        // GET: Files/Details/5
        public ActionResult Details(int id)
        {
            var files_View = (from x in db.tbl_files
                              join y in db.tbl_course on x.fileCourse equals y.courseID
                              where x.fileID == id
                              select new { x.fileID, x.fileDispName, x.fileCourse, x.filePath, x.fileType, x.fileDescription, y.courseName });

            return Json(files_View, JsonRequestBehavior.AllowGet);
        }

        // GET: Files/Create
        
        // POST: Files/Create
        [HttpPost]
        public ActionResult Create(fileViewModel vf)
        {
            try
            {
                tbl_files files = savefile(vf);
                if (files != null )
                {
                    db.tbl_files.Add(files);
                    
                    db.SaveChanges();
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json("Failed to upload file Please rename the file to smaller name and reupload it", JsonRequestBehavior.AllowGet);
                }
                
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Files/Edit/5
        

        // POST: Files/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, fileViewModel vf)
        {
            try
            {
                // TODO: Add update logic here
                var oldfile = db.tbl_files.SingleOrDefault(x => x.fileID == id);
                if (vf.file!=null && oldfile.fileDispName != Path.GetFileName(vf.file.FileName))
                {

                    delete_file(oldfile.filePath);
                   var newfile = editfile(vf);
                    oldfile.fileCourse = newfile.fileCourse;
                    oldfile.fileDispName = newfile.fileDispName;
                    oldfile.filePath = newfile.filePath;
                    oldfile.fileType = newfile.fileType;
                    oldfile.fileDescription = newfile.fileDescription;
                    
                    db.SaveChanges();
                }
                else {
                    oldfile.fileDescription = vf.description;
                    oldfile.fileCourse = vf.courseID;
                    db.SaveChanges();
                }


                
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        
        // POST: Files/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var delete_files = db.tbl_files.SingleOrDefault(x => x.fileID == id);
                if (delete_file(delete_files.filePath))
                {
                    db.tbl_files.Remove(delete_files);
                    db.SaveChanges();
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }


        private bool delete_file(string filepath) {

            try
            {
                if (System.IO.File.Exists(filepath))
                {

                    System.IO.File.Delete(filepath);
                    return true;
                }

                return false;
            }
            catch (Exception ex) {

                return false;
            }
        }



        private tbl_files savefile(fileViewModel vf) {
            tbl_files f = null ;
            try
            {
                
                f = new tbl_files();
                f.fileID = 1;
                f.fileDispName = Path.GetFileName(vf.file.FileName);
                //if (f.fileDispName.Length > 50) { f = null; return f; }
                f.fileDescription = vf.description;
                f.fileCourse = vf.courseID;
                f.fileType = Path.GetExtension(vf.file.FileName);     
               
                string filepath = System.Web.HttpContext.Current.Server.MapPath($"../assets/files/{f.fileDispName}");

                if (!System.IO.File.Exists(filepath))
                {
                    vf.file.SaveAs(filepath);
                    f.filePath = filepath;
                    return f;
                }
                else
                {
                    f = null;
                    return f;
                }
            }
            catch (Exception ex) {
                
                return f;

            }
        }
        private tbl_files editfile(fileViewModel vf)
        {
            tbl_files f = null;
            try
            {

                f = new tbl_files();
                f.fileID = 1;
                f.fileDispName = Path.GetFileName(vf.file.FileName);
                //if (f.fileDispName.Length > 50) { f = null; return f; }
                f.fileDescription = vf.description;
                f.fileCourse = vf.courseID;
                f.fileType = Path.GetExtension(vf.file.FileName);

                string filepath = System.Web.HttpContext.Current.Server.MapPath($"../assets/files/{f.fileDispName}");
                filepath=filepath.Replace("\\Files", "");
                if (!System.IO.File.Exists(filepath))
                {
                    vf.file.SaveAs(filepath);
                    f.filePath = filepath;
                    return f;
                }
                else
                {
                    f = null;
                    return f;
                }
            }
            catch (Exception ex)
            {

                return f;

            }
        }

        public ActionResult DownloadAttachment(string filename)
        {
            try
            {
                var file = db.tbl_files.FirstOrDefault(x => x.fileDispName == filename);

                byte[] fileBytes = System.IO.File.ReadAllBytes(file.filePath);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.fileDispName);
            }
            catch (Exception ex) {

                return Json("Unable to Download File PLease Retry ERROR : "+ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        //http://localhost:1740/Files/DownloadAttachment?filename=quickstart-master.zip

    }
}
