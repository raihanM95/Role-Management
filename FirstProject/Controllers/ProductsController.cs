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
using PagedList;

namespace FirstProject.Controllers
{
    public class ProductsController : Controller
    {
        private DBContext db = new DBContext();
        private ProductViewModel _productViewModel = new ProductViewModel();
        private Product _product = new Product();
        private ClientProduct _clientProduct = new ClientProduct();
        private SupportProduct _supportProduct = new SupportProduct();

        // GET: Products
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            
            _productViewModel.Products = db.Products.OrderBy(n => n.Name).ToPagedList(pageNumber, pageSize);
            return View(_productViewModel);
        }

        [HttpPost]
        public JsonResult ClientDetails(int id)
        {
            List<Users> clientList = new List<Users>();
            var clients = db.ClientProducts.Include(c => c.Users).Where(i => i.ProductId == id).ToList();
            foreach (var client in clients)
            {
                if (clientList.Count > 0)
                {
                    if (clientList.Where(i => i.ID == client.UsersId).ToList().Count == 0)
                    {
                        clientList.AddRange(db.Users.Where(i => i.ID == client.UsersId));
                    }
                }
                else
                {
                    clientList.AddRange(db.Users.Where(i => i.ID == client.UsersId));
                }
            }
            return Json(clientList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SupportDetails(int id)
        {
            List<Users> supportList = new List<Users>();
            var supports = db.SupportProducts.Include(s => s.Users).Where(i => i.ProductId == id).ToList();
            foreach (var support in supports)
            {
                if (supportList.Count > 0)
                {
                    if (supportList.Where(i => i.ID == support.UsersId).ToList().Count == 0)
                    {
                        supportList.AddRange(db.Users.Where(i => i.ID == support.UsersId));
                    }
                }
                else
                {
                    supportList.AddRange(db.Users.Where(i => i.ID == support.UsersId));
                }
            }
            return Json(supportList, JsonRequestBehavior.AllowGet);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ProductViewModel productViewModel = new ProductViewModel();

            ViewBag.ClientId = new SelectList(db.Users.Where(c => c.UserType == "Client"), "ID", "Name");
            ViewBag.SupportId = new SelectList(db.Users.Where(s => s.UserType == "Support"), "ID", "Name");
            
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                // Product name is already Exist
                var isExist = IsProductNameExist(productViewModel.Name);
                if (isExist)
                {
                    ModelState.AddModelError("ProductNameExist", "Product name already exist");
                    return View(productViewModel);
                }

                _product.Name = productViewModel.Name;
                db.Products.Add(_product);
                db.SaveChanges();

                Product aProduct = db.Products.Where(p => p.Name == productViewModel.Name).SingleOrDefault();

                if (productViewModel.ClientIds != null)
                {
                    foreach (var id in productViewModel.ClientIds)
                    {
                        _clientProduct.ProductId = aProduct.ID;
                        _clientProduct.UsersId = id;

                        db.ClientProducts.Add(_clientProduct);
                        db.SaveChanges();
                    }
                }
                if(productViewModel.SupportIds != null)
                {
                    foreach (var id in productViewModel.SupportIds)
                    {
                        _supportProduct.ProductId = aProduct.ID;
                        _supportProduct.UsersId = id;

                        db.SupportProducts.Add(_supportProduct);
                        db.SaveChanges();
                    }
                }
                
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Users.Where(c => c.UserType == "Client"), "ID", "Name", productViewModel.ClientIds);
            ViewBag.SupportId = new SelectList(db.Users.Where(s => s.UserType == "Support"), "ID", "Name", productViewModel.SupportIds);
            
            return View(productViewModel);
        }

        [NonAction]
        public bool IsProductNameExist(string name)
        {
            var p = this.db.Products.FirstOrDefault(n => n.Name == name);
            return p != null;
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            List<Users> clientList = new List<Users>();
            List<Users> supportList = new List<Users>();
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            
            if (product == null)
            {
                return HttpNotFound();
            }

            var clients = db.ClientProducts.Include(c => c.Users).Where(i => i.ProductId == id).ToList();
            if(clients != null)
            {
                foreach (var client in clients)
                {
                    if (clientList.Count > 0)
                    {
                        if (clientList.Where(i => i.ID == client.UsersId).ToList().Count == 0)
                        {
                            clientList.AddRange(db.Users.Where(i => i.ID == client.UsersId));
                        }
                    }
                    else
                    {
                        clientList.AddRange(db.Users.Where(i => i.ID == client.UsersId));
                    }
                }

                //int index = 0;
                foreach (var client in clientList)
                {
                    //_productViewModel.ClientIds[index] = client.ID;
                    ViewBag.ClientId = new SelectList(db.Users.Where(s => s.UserType == "Client"), "ID", "Name", client.ID);
                }
                //index++;
            }
            else
            {
                ViewBag.ClientId = new SelectList(db.Users.Where(c => c.UserType == "Client"), "ID", "Name");
            }


            var supports = db.SupportProducts.Include(s => s.Users).Where(i => i.ProductId == id).ToList();
            if(supports != null)
            {
                foreach (var support in supports)
                {
                    if (supportList.Count > 0)
                    {
                        if (supportList.Where(i => i.ID == support.UsersId).ToList().Count == 0)
                        {
                            supportList.AddRange(db.Users.Where(i => i.ID == support.UsersId));
                        }
                    }
                    else
                    {
                        supportList.AddRange(db.Users.Where(i => i.ID == support.UsersId));
                    }
                }

                //int index2 = 0;
                foreach (var support in supportList)
                {
                    //_productViewModel.SupportIds[index] = support.ID;
                    ViewBag.SupportId = new SelectList(db.Users.Where(s => s.UserType == "Support"), "ID", "Name", support.ID);
                }
                //index2++;
            }
            else
            {
                ViewBag.SupportId = new SelectList(db.Users.Where(s => s.UserType == "Support"), "ID", "Name");
            }

            _productViewModel.ID = product.ID;
            _productViewModel.Name = product.Name;

            //_productViewModel.ClientIds = (clientList.Select(i => i.ID)).ToArray();
            //_productViewModel.SupportIds = (supportList.Select(i => i.ID)).ToArray();

            //_productViewModel.SupportSelectListItems = db.Users.Where(c => c.UserType == "Client").Select(c => new SelectListItem()
            //{
            //    Value = c.ID.ToString(),
            //    Text = c.Name
            //});

            //IEnumerable<SelectListItem> EntityList = db.Users.ToList().Where(u => u.UserType.Equals("Client")).
            //Select(c => new SelectListItem
            //{
            //    Value = c.ID.ToString(),
            //    Text = c.Name,
            //});

            //ViewBag.ClientId = new SelectList(db.Users.Where(c => c.UserType == "Client"), "ID", "Name", _productViewModel.ClientIds);
            //ViewBag.SupportId = new SelectList(db.Users.Where(s => s.UserType == "Support"), "ID", "Name", _productViewModel.SupportIds);
            return View(_productViewModel);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                _product.ID = productViewModel.ID;
                _product.Name = productViewModel.Name;
                db.Entry(_product).State = EntityState.Modified;
                db.SaveChanges();

                var client = db.ClientProducts.Where(p => p.ProductId == productViewModel.ID);
                if(client != null)
                {
                    foreach(var cli in client)
                    {
                        _clientProduct.ID = cli.ID;
                        //_clientProduct.UsersId = productViewModel.ClientIds

                    }
                    //foreach (var i in productViewModel.ClientIds)
                    //{
                    //    //_clientProduct.ID = tid;
                    //    _clientProduct.UsersId = i;


                    //    db.Entry(_clientProduct).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //}
                }
                else
                {
                    foreach (var id in productViewModel.ClientIds)
                    {
                        _clientProduct.ProductId = productViewModel.ID;
                        _clientProduct.UsersId = id;

                        db.ClientProducts.Add(_clientProduct);
                        db.SaveChanges();
                    }
                }

                var support = db.SupportProducts.Where(p => p.ProductId == productViewModel.ID);
                if(support != null)
                {
                    
                }
                else
                {
                    foreach (var id in productViewModel.SupportIds)
                    {
                        _supportProduct.ProductId = productViewModel.ID;
                        _supportProduct.UsersId = id;

                        db.SupportProducts.Add(_supportProduct);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            return View(productViewModel);
        }

        // POST: Products/Delete/5
        public ActionResult Delete(int id)
        {
            ClientProduct clientProducts = db.ClientProducts.Where(i => i.ProductId == id).FirstOrDefault();
            SupportProduct supportProducts = db.SupportProducts.Where(i => i.ProductId == id).FirstOrDefault();
            if (clientProducts != null || supportProducts != null)
            {
                TempData["Message"] = "Can not delete this product!";
            }
            else
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
            }
            
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
