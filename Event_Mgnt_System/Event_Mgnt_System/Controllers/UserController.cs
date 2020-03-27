﻿using Event_Mgnt_System.Models;
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
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
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
            return RedirectToAction("WaitPage");

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

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult UserHome()
        {
            return View();
        }
    }
}