using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jamiaAPP.Models;
using jamiaAPP.Filters;
namespace jamiaAPP.Controllers
{
    [RoleBasedAuthorize]
    public class UserController : Controller
    {
        private JamiaRDBEntities db;
        public UserController()
        {
            db = new JamiaRDBEntities();
        }

        
        public ActionResult Index()
        {
            Session["page"] = "User Management";
            return View();
            
        }

        [HttpGet]
        public JsonResult List() {

            var user = (from u in db.tbl_user join c in db.tbl_campus on u.CID equals c.campusID join b in db.tbl_batch on u.BID equals b.batchID select new {

                u.userID,
                u.userName,
                u.userPassword,
                u.userRole,
                u.userDisplayName,
                u.userCellNumber,
                
                c.campusName,
                b.batchID,


            });

            return Json(user, JsonRequestBehavior.AllowGet);
        }
        
        
        // GET: User/Details/5
        public JsonResult Details(int id)
        {
            var user = (from u in db.tbl_user
                        join c in db.tbl_campus on u.CID equals c.campusID
                        join b in db.tbl_batch on u.BID equals b.batchID
                        where u.userID==id
                        select new
                        {

                            u.userID,
                            u.userName,
                            u.userPassword,
                            u.userRole,
                            u.userDisplayName,
                            u.userCellNumber,
                            c.campusID,
                            c.campusName,
                            b.batchID,


                        });

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getPassword() {


            return Json(Guid.NewGuid().ToString().Substring(0, 5), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        // POST: User/Create
        public JsonResult Create(tbl_user user)
        {
            try
            {
                db.tbl_user.Add(user);
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
        }

        
        
        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, tbl_user user)
        {
            try
            {
                var olduser = db.tbl_user.SingleOrDefault(x => x.userID == id);
                if (olduser != null) {

                    olduser.userName = user.userName;
                    olduser.userPassword = user.userPassword;
                    olduser.userDisplayName = user.userDisplayName;
                    olduser.userCellNumber = user.userCellNumber;
                    olduser.userRole = user.userRole;
                    olduser.BID = user.BID;
                    olduser.CID = user.CID;

                    db.SaveChanges();
                    return Json("success", JsonRequestBehavior.AllowGet);

                }


                return Json("user not found", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        

        // POST: User/Delete/5
        
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here

                var delete_user = db.tbl_user.SingleOrDefault(x => x.userID == id);
                db.tbl_user.Remove(delete_user);
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
