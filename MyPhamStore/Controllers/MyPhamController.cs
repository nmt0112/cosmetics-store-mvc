using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPhamStore.Models;
using PagedList;

namespace MyPhamStore.Controllers
{
    public class MyPhamController : Controller
    {
        // GET: MyPham
        //public ActionResult Index()
        //{
        //    var context = new MyPhamModelContext();
        //    return View(context.MyPhams.ToList());
        //}

        public ActionResult Index(int? page, int? categoryId, string searchString)
        {

            var context = new MyPhamModelContext();
            int pageSize = 6;
            int pageIndex = page.HasValue ? page.Value : 1;
            var listProduct = context.MyPhams.ToList();

            // Filter by categoryId, if specified
            if (categoryId.HasValue)
            {
                listProduct = listProduct.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            // Search by searchString, if specified
            if (!string.IsNullOrEmpty(searchString))
            {
                listProduct = listProduct.Where(p => p.Category.CategoryName.Contains(searchString) || p.Brand.BrandName.Contains(searchString) || p.Name.Contains(searchString)).ToList();
            }

            var result = listProduct.ToPagedList(pageIndex, pageSize);
            return View("Index", result);
        }

        //public ActionResult GetMyPhamCategory(int id)
        //{
        //    var context = new MyPhamModelContext();
        //    return View("Index", context.MyPhams.Where(p => p.CategoryId == id).ToList());
        //}
        public ActionResult GetMyPhamCategory(int id, int? page)
        {
            var context = new MyPhamModelContext();
            var listProduct = context.MyPhams.Where(p => p.CategoryId == id).ToList();

            int pageSize = 10;
            int pageIndex = page.HasValue ? page.Value : 1;
            var result = listProduct.ToPagedList(pageIndex, pageSize);

            return View("Index", result);
        }

        //public ActionResult GetCategory()
        //{
        //    var context = new MyPhamModelContext();
        //    var listCategory = context.Categories.ToList();
        //    return PartialView(listCategory);
        //}
        public ActionResult GetCategory()
        {
            var context = new MyPhamModelContext();
            var listCategory = context.Categories.ToList();
            return PartialView("GetCategory", listCategory);
        }

        public ActionResult GetMyPhamBrand(int id, int? page)
        {
            var context = new MyPhamModelContext();
            var listBrand = context.MyPhams.Where(p => p.BrandId == id).ToList();

            int pageSize = 10;
            int pageIndex = page.HasValue ? page.Value : 1;
            var result = listBrand.ToPagedList(pageIndex, pageSize);

            return View("Index", result);
        }

        //public ActionResult GetMyPhamBrand(int id)
        //{
        //    var context = new MyPhamModelContext();
        //    return View("Index", context.MyPhams.Where(p => p.BrandId == id).ToList());
        //}

        //public ActionResult GetBrand()
        //{
        //    var context = new MyPhamModelContext();
        //    var listBrand = context.Brands.ToList();
        //    return PartialView(listBrand);
        //}
        public ActionResult GetBrand()
        {
            var context = new MyPhamModelContext();
            var listBrand = context.Brands.ToList();
            return PartialView("GetBrand", listBrand);
        }

        public ActionResult Details(int id)
        {
            var context = new MyPhamModelContext();
            var firstMyPham = context.MyPhams.FirstOrDefault(p => p.Id == id);
            if (firstMyPham == null)
                return HttpNotFound("Không tìm thấy mã sản phẩm này!");
            return View(firstMyPham);
        }

        //public ActionResult Search(string searchString)
        //{
        //    var context = new MyPhamModelContext();
        //    var results =
        //        (from m in context.MyPhams
        //         where
        //         m.Brand.BrandName.Contains(searchString)
        //         || m.Category.CategoryName.Contains(searchString)
        //         || m.Name.Contains(searchString)
        //         select m);
        //    if (results.Count() > 0)
        //        return View("Index", results);
        //    return HttpNotFound("TThông tin tìm kiếm không tồn tại, xin cảm ơn");
        //}
        public ActionResult Search(string searchString, int? page)
        {
            var context = new MyPhamModelContext();
            var results = context.MyPhams
                .Where(m => m.Brand.BrandName.Contains(searchString) 
                || m.Category.CategoryName.Contains(searchString) 
                || m.Name.Contains(searchString))
                .OrderBy(p => p.Id)
                .ToPagedList(page ?? 1, 10);

            if (results.Count() > 0)
                return View("Index", results);

            return HttpNotFound("Thông tin tìm kiếm chưa có. Xin cảm ơn");
        }
    }
}