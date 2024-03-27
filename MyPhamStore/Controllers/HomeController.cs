using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPhamStore.Models;

namespace MyPhamStore.Controllers
{
    public class HomeController : Controller
    {
        MyPhamModelContext context = new MyPhamModelContext();
        public ActionResult Index()
        {
            HomeModels obj = new HomeModels();
            obj.listBarnds = context.Brands.ToList();
            obj.listMyPham = context.MyPhams.ToList();
            obj.listCategory = context.Categories.ToList();
            return View(obj);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}