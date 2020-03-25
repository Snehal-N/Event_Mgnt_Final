using Event_Mgnt_System.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Event_Mgnt_System.Controllers
{
    public class UserController : Controller
    {
        EventDBEntities1 db = new EventDBEntities1();

        // GET: User
        public ActionResult Index(int ?page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var list = db.Events.Where(x => x.Status == 0).OrderByDescending(x => x.Event_ID).ToList();

            IPagedList<Event> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
            
        }
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(Registration uvm)
        {
            Registration r = new Registration();
            r.User_Name = uvm.User_Name;
            r.Email_ID = uvm.Email_ID;
            r.Password = uvm.Password;
            r.ConfirmPassword = uvm.ConfirmPassword;
            r.City = uvm.City;
            r.ContactNumber = uvm.ContactNumber;
            db.Registrations.Add(r);
            db.SaveChanges();
          return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Registration avm)
        {
           Registration ad = db.Registrations.Where(x => x.User_Name == avm.User_Name && x.Password == avm.Password).SingleOrDefault();
            if (ad != null)
            {

                Session["User_ID"] = ad.User_ID.ToString();
                return RedirectToAction("UserHome");

            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
            return View();
        }

        public ActionResult UserHome()
        {
            return View();
        }
    }
}