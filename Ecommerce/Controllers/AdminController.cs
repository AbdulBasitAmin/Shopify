using Ecommerce.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class AdminController : Controller
    {
        ecommerceEntities db = new ecommerceEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(tbl_admin adm)
        {
            tbl_admin ad  = db.tbl_admin.Where(x => x.ad_name == adm.ad_name && x.ad_password == adm.ad_password).SingleOrDefault();

            if (ad!=null)
            {

                Session["ad_id"] = ad.ad_id.ToString();
                return RedirectToAction("Admin_Panel");


            }
            else
            {
                ViewBag.error = "Invalid Username or Password";
            }
            
            return View();
        }

        [HttpGet]
        public ActionResult Admin_Panel()
        {


            TempData["category"] = db.tbl_category.Count();
            TempData.Keep();


            TempData["product"] = db.tbl_product.Count();
            TempData.Keep();



            TempData["user"] = db.tbl_user.Count();
            TempData.Keep();


            return View();
        }
        public ActionResult pRODUCTS()
        {
         




            return View();
        }

        [HttpGet]
        public ActionResult Add_Category()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]

        public ActionResult Add_Category(tbl_category cat, HttpPostedFileBase imgfile)
        {
            tbl_admin ad = new tbl_admin();

            string path = uploadimage(imgfile);

            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded";

            }
            else
            {
                tbl_category ca = new tbl_category();
                ca.cat_name = cat.cat_name;
                ca.cat_image = path;
                ca.cat_status = 1;

                ca.ad_id_fk =Convert.ToInt32(Session["ad_id"].ToString());
                db.tbl_category.Add(ca);
                db.SaveChanges();
                return RedirectToAction("ViewCategory");

            }






            return View();
        }


        public ActionResult ViewAllProducts()
        {

            List<tbl_product> pro_list = db.tbl_product.ToList();

            List<ProductViewModel> pvm_list = pro_list.Select(x => new ProductViewModel
            {
                cat_id = x.tbl_category.cat_id,
                cat_name = x.tbl_category.cat_name,
                pro_id = x.pro_id,
                pro_name = x.pro_name,
                pro_desc = x.pro_desc,
                pro_price = x.pro_price,
                u_id = x.tbl_user.u_id,
                u_name = x.tbl_user.u_name,
            }
            ).ToList();

            return View(pvm_list);
        }
        public ActionResult RegisteredUser()
        {


            List<tbl_user> user_list = db.tbl_user.ToList(); 


            return View(user_list);
        }


            public ActionResult ViewCategory(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_category> cate = list.ToPagedList(pageindex, pagesize);

            return View(cate);
        }

        [HttpGet]

        public ActionResult Edit(int? id)
        {

            tbl_category ca = db.tbl_category.Where(x => x.cat_id == id).SingleOrDefault();
            return View(ca);
        }



        [HttpPost]

        public ActionResult Edit(int? id, tbl_category cat, HttpPostedFileBase imgFile)
        {
            string path = uploadimage(imgFile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded";
            }

            else
            {

                tbl_category ca = db.tbl_category.Where(x => x.cat_id == id).SingleOrDefault();
                ca.cat_name = cat.cat_name;
                ca.cat_status = cat.cat_status;
                ca.cat_image = path;
                db.SaveChanges();
            }


            return RedirectToAction("ViewCategory");
        }

        public ActionResult Delete(int? id, tbl_category cat)
        {
            tbl_category ca = db.tbl_category.Where(x => x.cat_id == id).SingleOrDefault();
            db.tbl_category.Remove(ca);
            db.SaveChanges();

            return RedirectToAction("ViewCategory");
        }


        //public ActionResult Admin_Panel()
        //{
        //    return View();
        //}

        public string uploadimage(HttpPostedFileBase file)

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