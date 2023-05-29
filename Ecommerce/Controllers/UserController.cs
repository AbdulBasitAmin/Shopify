using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Ecommerce.Controllers
{
    public class UserController : Controller
    {
        ecommerceEntities db = new ecommerceEntities();
        // GET: User
        public ActionResult Index(int ? page)
        {


            // changes made
            if (TempData["cart"] !=null)
            {
                float x = 0;
                List<cart> li2 = TempData["cart"] as List<cart>;

                foreach (var item in li2)
                {
                    x = Convert.ToInt32(item.o_bill);


                }

                TempData["total"] = x;

            }
            TempData.Keep();

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_category> cate = list.ToPagedList(pageindex, pagesize);

            return View(cate);
            
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(tbl_user uvm)
        {
            tbl_user us = db.tbl_user.Where(x => x.u_name == uvm.u_name && x.u_password == uvm.u_password).SingleOrDefault();

            if (us != null)
            {

                Session["us_id"] = us.u_id.ToString();

                Session["us_name"] = us.u_name.ToString();
                return RedirectToAction("Index");


            }
            else
            {
                ViewBag.error = "Invalid Username or Password";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(tbl_user us, HttpPostedFileBase imgfile)
        {
        
            string path = uploadimage(imgfile);

            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded";

            }
            else
            {
                tbl_user u = new tbl_user();
                u.u_name = us.u_name;
                u.u_email = us.u_email;
                u.u_password = us.u_password;
                u.u_image = path;
                u.u_contact = us.u_contact;


                db.tbl_user.Add(u);
                db.SaveChanges();
                return RedirectToAction("Login");

            }


            return View();
        }
        [HttpGet]
        public ActionResult Create_Add()
        {


            if (Session["us_id"] == null)
            {
                return RedirectToAction("Login");
            }

            List<tbl_category> li = db.tbl_category.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");

            return View();
        }
        [HttpPost]
        public ActionResult Create_Add(tbl_product pr , HttpPostedFileBase imgfile)
        {
            List<tbl_category> li = db.tbl_category.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");
            string path = uploadimage(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded";
            }
            else
            {
                tbl_product pro = new tbl_product();
                pro.pro_name = pr.pro_name;
                pro.pro_price = pr.pro_price;
                pro.pro_desc = pr.pro_desc;
                pro.pro_image = path;
                pro.cat_id_fk = pr.cat_id_fk;
                pro.us_id_fk = Convert.ToInt32(Session["us_id"].ToString());
                db.tbl_product.Add(pro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult DisplayAdd(int ? id , int ? page)
        {

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var list = db.tbl_product.Where(x => x.cat_id_fk == id).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<tbl_product> pro = list.ToPagedList(pageindex, pagesize);

            return View(pro);
        }

        [HttpPost]
        public ActionResult DisplayAdd(int? id, int? page, string search)
        {

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var list = db.tbl_product.Where(x => x.pro_name.Contains(search)).OrderByDescending(x => x.pro_id).ToList();
         
            
            IPagedList<tbl_product> pro = list.ToPagedList(pageindex, pagesize);

            return View(pro);
        }

        public ActionResult ViewAdds(int ? id)
        {
            if (Session["us_id"] == null)
            {
                return RedirectToAction("Login");
            }
            product_details pr = new product_details();

            tbl_product p = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();
            pr.pro_id = p.pro_id;
            pr.pro_name = p.pro_name;
            pr.pro_image = p.pro_image;
            pr.pro_price = p.pro_price;
            pr.pro_desc = p.pro_desc;

            tbl_category cat = db.tbl_category.Where(x => x.cat_id == p.cat_id_fk).SingleOrDefault();

            pr.cat_name = cat.cat_name;


            tbl_user us = db.tbl_user.Where(x => x.u_id == p.us_id_fk).SingleOrDefault();

            pr.u_name = us.u_name;
            pr.u_image = us.u_image;
            pr.u_contact = us.u_contact;

            pr.us_id_fk = us.u_id;


            return View(pr);
        }
        public ActionResult User_Panel()
        {
            return View();
        }



        public ActionResult Add_Delete(int ? id)
        {

            tbl_product p = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();

            db.tbl_product.Remove(p);

            db.SaveChanges();
            return RedirectToAction("Index");

           
        }

        public ActionResult Signout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index");


        
        }

        public ActionResult Add_to_Cart(int? id)
        {

            tbl_product p = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();

            return View(p);
        }

        List<cart> li = new List<cart>();

        [HttpPost]

        public ActionResult Add_to_Cart(int? id,  tbl_product pr , string qty)
        {

            tbl_product p = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();
            cart ca = new cart();

            ca.pro_id = p.pro_id;
            ca.pro_name = p.pro_name;
            ca.pro_price = p.pro_price;
            ca.o_qty = Convert.ToInt32(qty);

            ca.o_bill = ca.pro_price * ca.o_qty;

            if (TempData["cart"] == null)
            {
                li.Add(ca);

                TempData["cart"] = li;
            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;

                li2.Add(ca);

                TempData["cart"] = li2;


            }
            TempData.Keep();

            return RedirectToAction("Index");
        }


        public ActionResult checkout()

        {
            TempData.Keep();

            return View();
        }

        [HttpPost]
        public ActionResult checkout(tbl_order or)

        {
            List<cart> li = TempData["cart"] as List<cart>;

            tbl_invoice iv = new tbl_invoice();
            iv.in_fk_user = Convert.ToInt32(Session["us_id"].ToString());

            iv.in_date = System.DateTime.Now;

            iv.in_totalbill = Convert.ToInt32(TempData["total"]);

            db.tbl_invoice.Add(iv);
            db.SaveChanges();


            foreach (var item in li)
            {

                tbl_order o = new tbl_order();
                o.o_fk_pro = item.pro_id;

                o.o_fk_invoice = iv.in_id;

                o.o_date = System.DateTime.Now;

                o.o_qty = item.o_qty;

                o.o_unitprice = item.pro_price;

                o.o_bill = item.o_bill;

                db.tbl_order.Add(o);

                db.SaveChanges();


            }

            TempData.Remove("Total");

            TempData.Remove("Cart");

            TempData["msg"] = "Transaction Successfully Completed.....................";


            TempData.Keep();

            return RedirectToAction("Index");
        }


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