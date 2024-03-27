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
    public class BrandController : Controller
    {
        private MyPhamModelContext db = new MyPhamModelContext();
        [Authorize(Roles = "Admin")]
        // GET: Admin/Brand
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Brand/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand brand)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Brands.Add(brand);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(brand);
            var context = new MyPhamModelContext();
            if (ModelState.IsValid)
            {
                if (brand.ImageFile != null && brand.ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(brand.ImageFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Content/ImageProducts/Brand"), fileName);
                    brand.ImageFile.SaveAs(filePath);
                    brand.Image = "" + fileName;
                }
                context.Brands.Add(brand);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            //return View(book);
            else
            {
                return RedirectToAction("Create");
            }
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Brand/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (brand.ImageFile != null && brand.ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(brand.ImageFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Content/ImageProducts/Brand"), fileName);
                    brand.ImageFile.SaveAs(filePath);
                    brand.Image = fileName;
                }

                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Brand/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
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
