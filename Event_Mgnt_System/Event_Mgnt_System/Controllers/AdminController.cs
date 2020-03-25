using Event_Mgnt_System.Models;
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
        EventDBEntities db = new EventDBEntities();
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
                return RedirectToAction("Create");

            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
            return View();
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