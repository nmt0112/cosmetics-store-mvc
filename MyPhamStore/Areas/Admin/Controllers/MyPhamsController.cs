using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyPhamStore.Models;

namespace MyPhamStore.Areas.Admin.Controllers
{
    public class MyPhamsController : Controller
    {
        private MyPhamModelContext db = new MyPhamModelContext();

        // GET: Admin/MyPhams
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var myPhams = db.MyPhams.Include(m => m.Brand).Include(m => m.Category);
            return View(myPhams.ToList());
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/MyPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyPham myPham = db.MyPhams.Find(id);
            if (myPham == null)
            {
                return HttpNotFound();
            }
            return View(myPham);
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/MyPhams/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName");
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Admin/MyPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyPham myPham)
        {
            var context = new MyPhamModelContext();
            if (ModelState.IsValid)
            {
                if (myPham.ImageFile != null && myPham.ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(myPham.ImageFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Content/ImageProducts/MyPham"), fileName);
                    myPham.ImageFile.SaveAs(filePath);
                    myPham.Image = "" + fileName;
                }
                context.MyPhams.Add(myPham);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", myPham.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", myPham.CategoryId);
            return View(myPham);
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/MyPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyPham myPham = db.MyPhams.Find(id);
            if (myPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", myPham.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", myPham.CategoryId);
            return View(myPham);
        }

        // POST: Admin/MyPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyPham myPham)
        {
            if (ModelState.IsValid)
            {
                if (myPham.ImageFile != null && myPham.ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(myPham.ImageFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Content/ImageProducts/MyPham"), fileName);
                    myPham.ImageFile.SaveAs(filePath);
                    myPham.Image = fileName;
                }

                db.Entry(myPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", myPham.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", myPham.CategoryId);
            return View(myPham);
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/MyPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyPham myPham = db.MyPhams.Find(id);
            if (myPham == null)
            {
                return HttpNotFound();
            }
            return View(myPham);
        }

        // POST: Admin/MyPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyPham myPham = db.MyPhams.Find(id);
            db.MyPhams.Remove(myPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
