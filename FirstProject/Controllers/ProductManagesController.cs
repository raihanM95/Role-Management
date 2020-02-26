using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstProject.DatabaseContext;
using FirstProject.Models;
using FirstProject.ViewModels;

namespace FirstProject.Controllers
{
    public class ProductManagesController : Controller
    {
        private DBContext db = new DBContext();
        private ClientProduct clientProduct;
        private SupportProduct supportProduct;

        // GET: ProductManages/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "ID", "Name");
            ViewBag.SupportId = new SelectList(db.Users.Where(s => s.UserType == "Support"), "ID", "UserName");
            ViewBag.ClientId = new SelectList(db.Users.Where(c => c.UserType == "Client"), "ID", "UserName");
            return View();
        }

        // POST: ProductManages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductManageViewModel productManageViewModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var cli in productManageViewModel.ClientProducts)
                {
                    clientProduct = new ClientProduct
                    {
                        UsersId = cli.UsersId,
                        ProductId = cli.ProductId
                    };
                    db.ClientProducts.Add(clientProduct);
                    db.SaveChanges();
                }

                foreach (var sup in productManageViewModel.SupportProducts)
                {
                    supportProduct = new SupportProduct
                    {
                        UsersId = sup.UsersId,
                        ProductId = sup.ProductId
                    };
                    db.SupportProducts.Add(supportProduct);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "ID", "Name", productManageViewModel.ProductId);
            ViewBag.SupportId = new SelectList(db.Users.Where(s => s.UserType == "Support"), "ID", "UserName", productManageViewModel.SupportId);
            ViewBag.ClientId = new SelectList(db.Users.Where(c => c.UserType == "Client"), "ID", "UserName", productManageViewModel.ClientId);
            return View(productManageViewModel);
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
