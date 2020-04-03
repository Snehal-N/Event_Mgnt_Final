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
            ModelState.Clear();
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
                ModelState.Clear();
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
            ModelState.Clear();
            return View();
        }

        //public ActionResult BookEvent()
        //{
        //    //List<Event> li = db.Events.ToList();
        //    //ViewBag.eventlist = new SelectList(li, "Event_ID", "Event_Type");

        //    return View();
        //}
        
        public ActionResult BookEvent(String Name)
        {
            ViewBag.Name = Name;
            

            return View();
        }

        [HttpPost]
        public ActionResult BookEvent(Booking_Events ev, String Name)
        {
            Booking_Events bv = new Booking_Events();
            Registration r = db.Registrations.Where(x => x.Email_ID == ev.email).Single();

            Event e = db.Events.Where(x => x.Event_Type == ev.Event_Type).Single();

            bv.Venue = ev.Venue;
            bv.Event_Date = ev.Event_Date;
            bv.Event_time = ev.Event_time;
            bv.Guest_Number = ev.Guest_Number;
            bv.email = ev.email;
            bv.Event_Type =Name;
            bv.Approval = "Wait";


            if(r!=null)
            {
                bv.User_ID = r.User_ID;
                bv.User_Name = r.User_Name;
            }
            if (e != null)
            {
                bv.Event_ID = e.Event_ID;
            }
           

            db.Booking_Events.Add(bv);
            db.SaveChanges();
            ModelState.Clear();
            return View();

   }

        public ActionResult WaitPage()
        {
            return View();        
        }

        public ActionResult Logout()
        {

            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index");
        }

        public ActionResult Profile(int ? page)
        {
            
            if (Session["User_ID"]!=null)
             {
                
              int uid = Convert.ToInt32(Session["User_ID"]);

                Registration r = db.Registrations.Where(x => x.User_ID == uid).Single();
                ViewBag.UserName = r.User_Name;
                ViewBag.city = r.City;
                ViewBag.Email = r.Email_ID;
                ViewBag.contact = r.ContactNumber;


          



            }
            if(Session["User_ID"]!=null)
            {
                int uid = Convert.ToInt32(Session["User_ID"]);
                int pagesize = 9, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

                var list = db.Booking_Events.Where(x => x.User_ID == uid).OrderByDescending(x => x.Event_Date).ToList();

                IPagedList<Booking_Events> stu = list.ToPagedList(pageindex, pagesize);


                return View(stu);


            }
        


            return View();


            
        }


       
       
        public ActionResult PackageSelection()
        {

           
            return View();


        }

        public ActionResult Add_Package(String Name)
        {
            int uid = Convert.ToInt32(Session["User_ID"]);

            var list = db.Booking_Events.Where(x => x.User_ID== uid).ToList();
            var list2=list.Find(x => x.Approval == "confir" && x.package_name == null);
            list2.package_name = Name;

            int biid = list2.Book_ID;
            db.Booking_Events.Add(list2);

            db.SaveChanges();
          Booking_Events ev=  db.Booking_Events.Where(x => x.Book_ID == biid).Single();

            db.Booking_Events.Remove(ev);
            db.SaveChanges();

          

           




            return RedirectToAction("UserHome");
        }

        public ActionResult Feedback()
        {
            return View();
        }

        public ActionResult ThanksFeed()
        {
            return View();
        }
        public ActionResult Thanks()
        {
            return View();
        }
        public ActionResult UserHome()
        {
            return View();
        }
    }
}