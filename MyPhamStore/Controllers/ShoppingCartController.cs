using Microsoft.AspNet.Identity;
using MyPhamStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyPhamStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public List<CartItem> GetCartItems()
        {
            var lstShoppingCart = Session["ShoppingCart"] as List<CartItem>;
            if (lstShoppingCart == null)
            {
                lstShoppingCart = new List<CartItem>();
                Session["ShoppingCart"] = lstShoppingCart;
            }
            return lstShoppingCart;
        }

        // GET: ShoppingCart
        public ActionResult Index()
        {
            List<CartItem> ShoppingCart = GetCartItems();
            ViewBag.TongTien = ShoppingCart.Sum(p => p.Price * p.Quantity);
            ViewBag.TongSoLuong = ShoppingCart.Sum(p => p.Quantity);
            return View(ShoppingCart);
        }
        public ActionResult CartSummary()
        {
            ViewBag.CartCount = GetCartItems().Sum(p => p.Quantity);
            return PartialView("CartSummary");
        }

        public RedirectToRouteResult AddToCart(int id)
        {
            MyPhamModelContext db = new MyPhamModelContext();
            List<CartItem> ShoppingCart = GetCartItems();
            CartItem findCartItem = ShoppingCart.FirstOrDefault(m => m.Id == id);
            if (findCartItem == null)
            {
                MyPham findMyPham = db.MyPhams.First(m => m.Id == id);
                CartItem newItem = new CartItem()
                {
                    Id = findMyPham.Id,
                    Name = findMyPham.Name,
                    Quantity = 1,
                    Image = findMyPham.Image,
                    Price = findMyPham.Price.Value
                };
                ShoppingCart.Add(newItem);
            }
            else
                findCartItem.Quantity++;
            return RedirectToAction("Index", "ShoppingCart");
        }

        public RedirectToRouteResult UpdateCart(int id, int txtQuantity)
        {
            var itemFind = GetCartItems().FirstOrDefault(p => p.Id == id);
            if (itemFind != null)
                itemFind.Quantity = txtQuantity;
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Order()
        {
            string currentUserId = User.Identity.GetUserId();
            MyPhamModelContext context = new MyPhamModelContext();
            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Order objOrder = new Order()
                    {
                        CustomerId = null,
                        OrderDate = DateTime.Now,
                        DeliveryDate = null,
                        isPaid = 1,
                        isComplete = 1
                    };
                    objOrder = context.Orders.Add(objOrder);
                    context.SaveChanges();

                    List<CartItem> listCartItem = GetCartItems();
                    foreach (var item in listCartItem)
                    {
                        OrderDetail ctdh = new OrderDetail()
                        {
                            OrderNo = objOrder.OrderNo,
                            MyPhamId = item.Id,
                            Quantity = item.Quantity,
                            Price = item.Price
                        };
                        context.OrderDetails.Add(ctdh);
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Content("Gap loi khi dat hang:" + ex.Message);
                }
            }
            Session["ShoppingCart"] = null;
            return RedirectToAction("ConfirmOrder", "ShoppingCart");
        }

        public ActionResult ConfirmOrder()
        {
            return View();
        }

        public ActionResult Delete()
        {
            Session["ShoppingCart"] = null;
            return RedirectToAction("Index", "ShoppingCart");
        }

        public ActionResult RemoveCartItem(int id)
        {
            List<CartItem> ShoppingCart = GetCartItems();
            CartItem itemToRemove = ShoppingCart.FirstOrDefault(m => m.Id == id);
            if (itemToRemove != null)
            {
                ShoppingCart.Remove(itemToRemove);
            }
            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}