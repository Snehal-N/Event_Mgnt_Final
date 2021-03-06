﻿using Event_Mgnt_System.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace Event_Mgnt_System.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        EventDBEntities1 db = new EventDBEntities1();
            [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tbl_admin avm)
        {
            tbl_admin ad = db.tbl_admin.Where(x => x.ad_username == avm.ad_username && x.ad_password == avm.ad_password).SingleOrDefault();
            if (ad != null)
            {

                Session["ad_id"] = ad.ad_id.ToString();
                return RedirectToAction("AdminHome");

            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
            return View();
        }
        public ActionResult AdminHome()
        {
            if (Session["ad_id"]!=null)
             {
                int aid = Convert.ToInt32(Session["ad_id"]);
                tbl_admin ta = db.tbl_admin.Where(x => x.ad_id == aid).Single();
                ViewBag.adname = ta.ad_username;
                return View();
            }
            else
            {
                return RedirectToAction("Login");

            }

         
        }
        public ActionResult Create()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(Event cvm,HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                Event ev = new Event();
                ev.Event_Type= cvm.Event_Type;
                ev.event_image= path;
          
               
                db.Events.Add(ev);
                db.SaveChanges();
                return RedirectToAction("ViewEvents");
            }
            return View();
        }


        public ActionResult ViewEvents(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
           
            var list = db.Events.Where(x => x.Status == 0).OrderByDescending(x=>x.Event_ID).ToList();
          
            IPagedList<Event> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);



        }


        public ActionResult DeleteEvent(int id)
        {
            Event e = db.Events.Where(x => x.Event_ID == id).Single();
            db.Events.Remove(e);
            db.SaveChanges();

            return RedirectToAction("ViewEvents");
        }
        public int bi;
        public ActionResult AdminApproval(int? page,int id=0)
        {
            
              bi = id;

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var list = db.Booking_Events.Where(x => x.Approval == "wait").OrderByDescending(x => x.Event_ID).ToList();

            IPagedList<Booking_Events> stu = list.ToPagedList(pageindex, pagesize);

            if (bi != 0)
            {
                Booking_Events e = db.Booking_Events.Where(x => x.Book_ID == bi).Single();
             
                e.Approval = "confir";

                db.Booking_Events.Add(e);
                db.SaveChanges();
                Booking_Events ei = db.Booking_Events.Where(x => x.Book_ID == bi).Single();
                db.Booking_Events.Remove(ei);
                db.SaveChanges();

            }


            return View(stu);

        }
      
        public ActionResult SeeFeedback(int? page)
        {


            

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var list = db.feedbacks.Where(x => x.Details !=null).OrderByDescending(x => x.Uid).ToList();

            IPagedList<feedback> stu = list.ToPagedList(pageindex, pagesize);

            return View(stu);
        }
       
       



        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }

    }
}