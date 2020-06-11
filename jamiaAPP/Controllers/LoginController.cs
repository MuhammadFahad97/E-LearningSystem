using jamiaAPP.viewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jamiaAPP.Models;
namespace jamiaAPP.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        // GET: UserLogin
        public ActionResult Login()
        {
            if (cookiefetch())
            {
                return RedirectToAction("Index", "User");


            }


            if (Session["UserID"] != null && Session["UserName"] != null)
            {

                return RedirectToAction("Index", "User");
            }
            Session["page"] = "Login";
            return View();
        }

        //called from Login view and authenticate user create session and if user not verified redirect to verifyemail view througn ajax promise

        public ActionResult LoginAuth(LoginCredentials lc)
        {
            using (JamiaRDBEntities db = new JamiaRDBEntities())
            {
                var user = db.tbl_user.SingleOrDefault(x => (x.userName == lc.userName) && (x.userPassword == lc.userPassword));
                if (user == null)
                {
                    user = db.tbl_user.SingleOrDefault(x => (x.userCellNumber== lc.userName) && (x.userPassword == lc.userPassword));
                }


                if (user == null) {
                  user =  db.tbl_user.SingleOrDefault(x => (x.userDisplayName == lc.userName) && (x.userPassword == lc.userPassword));
                }

                if (user != null)
                {


                    if (lc.is_remember == true)
                    {
                        if (user.userToken == null)
                        {
                            var token = Guid.NewGuid().ToString();
                            user.userToken = token;
                            db.SaveChanges();
                        }

                        RememberChecked(user.userDisplayName, user.userToken);

                    }


                        Session["UserID"] = user.userID;
                        Session["UserName"] = user.userDisplayName;
                        Session["UserRole"] = user.userRole;
                        return Json("success");

                    
                    
                }
                return Json("failed");
            }


        }
        //logsout user
        public ActionResult LogOut()
        {
            if (Session["UserID"] != null && Session["UserName"] != null)
            {
                Session["UserID"] = null;
                Session["UserName"] = null;
                Session["UserRole"] = null;
                try
                {
                    var c = HttpContext.Request.Cookies.Get("usertoken");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Set(c);
                    RedirectToAction("Login");
                }
                catch (Exception ex)
                {

                    RedirectToAction("Login");
                }
            }
            return RedirectToAction("Login");



        }



        public void RememberChecked(string userDispName, string userToken)
        {

            //         var authTicket = new FormsAuthenticationTicket(
            //  1,
            //  "local",  //user id
            //  DateTime.Now,
            //  DateTime.Now.AddMinutes(20),  // expiry
            //  true,  //true to remember
            //  userPassword, //roles 
            //  "/"
            //);

            //encrypt the ticket and add it to a cookie
            HttpCookie mycookie = new HttpCookie("usertoken");
            mycookie.Values["user"] = userDispName;
            mycookie.Values["token"] = userToken;
            mycookie.Expires = System.DateTime.Now.AddDays(1);
            Response.Cookies.Add(mycookie);



        }

        private bool cookiefetch()
        {

            try
            {

                HttpCookie authCookie = HttpContext.Request.Cookies.Get("usertoken");
                if (authCookie != null)
                {
                    string userName = authCookie.Values["user"];
                    string userToken = authCookie.Values["token"];
                    using (jamiaAPP.Models.JamiaRDBEntities db = new jamiaAPP.Models.JamiaRDBEntities())
                    {
                        var user = db.tbl_user.SingleOrDefault(x => ((x.userDisplayName == userName) && (x.userToken == userToken)));
                        if (user != null)
                        {

                            Session["UserID"] = user.userID;
                            Session["UserName"] = user.userDisplayName;
                            Session["UserRole"] = user.userRole;

                            return true;
                        }
                    }

                }

                // Decrypts the FormsAuthenticationTicket that is held in the cookie's .Value property.
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }



    }
}